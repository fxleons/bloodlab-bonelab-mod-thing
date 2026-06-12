using System.Collections.Generic;
using UnityEngine;

namespace BloodLabMod.Core
{
    public static class PuddleManager
    {
        private static List<BloodPuddle> puddles = new List<BloodPuddle>();

        public static void Initialize() { }

        // Seed or grow puddles where droplets land
        public static void SeedPuddle(Vector3 worldPos, float volume, Vector3 normal)
        {
            // find nearest puddle within merge distance
            float mergeDist = 1.0f;
            BloodPuddle nearest = null;
            float best = float.MaxValue;
            foreach (var p in puddles)
            {
                var d = Vector3.Distance(p.Position, worldPos);
                if (d < best && d < mergeDist)
                {
                    best = d; nearest = p;
                }
            }
            if (nearest != null)
            {
                nearest.AddVolume(volume);
            }
            else
            {
                if (puddles.Count > ConfigManager.Settings.MaxPuddles) return;
                var p = new BloodPuddle(worldPos, normal, volume);
                puddles.Add(p);
            }
        }

        public static void UpdateAll(float dt)
        {
            for (int i = puddles.Count - 1; i >= 0; --i)
            {
                var p = puddles[i];
                p.Update(dt);
                if (p.IsExpired)
                {
                    p.Destroy();
                    puddles.RemoveAt(i);
                }
            }
        }
    }
}
