using UnityEngine;

namespace BloodLabMod.Core
{
    // Simple puddle representation: uses a decal prefab and scales over time
    public class BloodPuddle
    {
        public Vector3 Position { get; private set; }
        private Vector3 normal;
        private float volume;
        private GameObject decalGO;
        private float age;
        public bool IsExpired => age > ConfigManager.Settings.DecalLifetime * 2f;

        public BloodPuddle(Vector3 position, Vector3 normal, float initialVolume)
        {
            Position = position;
            this.normal = normal;
            volume = initialVolume;
            age = 0f;
            SpawnDecal();
        }

        private void SpawnDecal()
        {
            decalGO = RuntimeBloodAssets.CreateBloodPuddle();
            if (decalGO == null) return;
            decalGO.transform.position = Position + normal * 0.01f;
            decalGO.transform.rotation = Quaternion.LookRotation(normal);
            UpdateVisual();
        }

        public void AddVolume(float v)
        {
            volume += v;
            UpdateVisual();
        }

        public void Update(float dt)
        {
            age += dt;
            // darken as dries
            if (decalGO != null)
            {
                var rend = decalGO.GetComponent<Renderer>();
                if (rend != null && rend.material != null)
                {
                    var c = ConfigManager.Settings.BloodColor;
                    var dry = Mathf.Clamp01(age * ConfigManager.Settings.BloodDryingSpeed);
                    rend.material.color = Color.Lerp(c, Color.black, dry);
                }
            }
        }

        private void UpdateVisual()
        {
            if (decalGO == null) return;
            float scale = Mathf.Clamp(0.1f + volume * 0.5f, 0.1f, 5f);
            decalGO.transform.localScale = new Vector3(scale, 1f, scale);
        }

        public void Destroy()
        {
            if (decalGO != null) GameObject.Destroy(decalGO);
        }
    }
}
