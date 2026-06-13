using UnityEngine;

namespace BloodLabMod.Core
{
    public static class DecalManager
    {
        public static void Initialize() { }

        public static void SpawnWallSplat(Vector3 pos, Vector3 normal, float size, float intensity)
        {
            if (!BloodConfig.EnableBloodDecals) return;
            var go = RuntimeBloodAssets.CreateBloodDecal();
            if (go == null) return;
            go.transform.position = pos + normal * 0.01f;
            go.transform.rotation = Quaternion.LookRotation(normal);
            go.transform.localScale = Vector3.one * Mathf.Clamp(size * intensity, 0.2f, 6f);
            var rend = go.GetComponent<Renderer>();
            if (rend != null && rend.material != null)
                rend.material.color = ConfigManager.Settings.BloodColor;
            GameObject.Destroy(go, ConfigManager.Settings.DecalLifetime);
        }
    }
}
