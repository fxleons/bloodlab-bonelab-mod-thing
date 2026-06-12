using System.Collections.Generic;
using UnityEngine;

namespace BloodLabMod.Core
{
    // Handles physics-based blood droplets
    public static class DropletManager
    {
        private static List<BloodDroplet> droplets = new List<BloodDroplet>();

        public static void Initialize() { }

        public static void SpawnGore(Vector3 pos, Vector3 normal, float intensity)
        {
            int count = Mathf.CeilToInt(Mathf.Clamp(5f * intensity * ConfigManager.Settings.BloodMultiplier, 1f, 50f));
            for (int i = 0; i < count; i++)
            {
                SpawnDroplet(pos + Random.insideUnitSphere * 0.05f, Random.onUnitSphere * 0.5f + -normal * 1f, intensity);
            }
        }

        public static void SpawnDrip(Vector3 pos, Vector3 normal, float amount)
        {
            if (droplets.Count > ConfigManager.Settings.MaxDroplets) return;
            if (amount <= 0.0001f) return;
            SpawnDroplet(pos + normal * 0.01f, -normal * Random.Range(0.1f, 0.3f), amount);
        }

        private static void SpawnDroplet(Vector3 pos, Vector3 velocity, float volume)
        {
            var prefabPath = "BloodPrefabs/BloodDroplet";
            var go = PoolManager.Get(prefabPath);
            if (go == null) return;
            go.transform.position = pos;
            go.transform.rotation = Random.rotation;
            go.SetActive(true);
            var bd = go.GetComponent<BloodDroplet>();
            if (bd == null) bd = go.AddComponent<BloodDroplet>();
            bd.Initialize(velocity, volume);
            droplets.Add(bd);
        }

        public static void UpdateAll(float dt)
        {
            for (int i = droplets.Count - 1; i >= 0; --i)
            {
                var d = droplets[i];
                d.Simulate(dt);
                if (d.IsSettled)
                {
                    droplets.RemoveAt(i);
                    PuddleManager.SeedPuddle(d.Position, d.Volume, d.LastNormal);
                    PoolManager.Release("BloodPrefabs/BloodDroplet", d.gameObject);
                }
            }
        }
    }
}
