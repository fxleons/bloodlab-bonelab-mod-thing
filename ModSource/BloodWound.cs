using UnityEngine;

namespace BloodLabMod.Core
{
    // Represents a persistent wound attached to a body part
    public class BloodWound
    {
        public Transform Parent { get; private set; }
        public Vector3 LocalPosition { get; private set; }
        public Vector3 Normal { get; private set; }
        public float Damage { get; private set; }
        public string BodyPart { get; private set; }

        private float bleedIntensity;
        private float lifeTimer;
        private float maxLife;
        public bool IsFinished => bleedIntensity <= 0f && lifeTimer > maxLife;

        public BloodWound(Transform parent, Vector3 localPos, Vector3 normal, float damage, string bodyPart)
        {
            Parent = parent;
            LocalPosition = localPos;
            Normal = normal;
            Damage = damage;
            BodyPart = bodyPart;
            bleedIntensity = Mathf.Clamp01(damage * 0.1f);
            maxLife = Mathf.Max(30f, damage * 10f) * ConfigManager.Settings.BleedingDurationMultiplier;
            lifeTimer = 0f;
            // spawn initial gush
            var worldPos = Parent.TransformPoint(LocalPosition);
            DropletManager.SpawnGore(worldPos, Normal, Mathf.Clamp01(bleedIntensity));
        }

        public void Update(float dt)
        {
            lifeTimer += dt;
            // bleed over time: rate reduces as bleedIntensity decays
            if (bleedIntensity > 0f)
            {
                var amount = bleedIntensity * dt * (1f - (lifeTimer / maxLife));
                DropletManager.SpawnDrip(Parent.TransformPoint(LocalPosition), Normal, amount);
                bleedIntensity = Mathf.Max(0f, bleedIntensity - dt * 0.01f);
            }
        }
    }
}
