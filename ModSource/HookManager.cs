using UnityEngine;

namespace BloodLabMod.Core
{
    // Central place to hook into BONELAB damage systems. This file provides
    // example hooks and utility wrappers. Integrators should adapt these
    // hooks to BONELAB's specific damage event APIs.
    public static class HookManager
    {
        public static void Initialize()
        {
            // Examples: wire into BONELAB's damage dispatcher.
            // This is intentionally generic: the real game has methods such as
            // "HitManager.RegisterOnDamage" or similar. Replace with actual hooks.
        }

        // Call this from actual hit handling (bullets, melee, explosions)
        public static void OnEntityHit(Transform entity, Vector3 hitWorldPos, Vector3 hitNormal, float damage, string bodyPart, GameObject hitSource)
        {
            // create wound
            var localPos = entity.InverseTransformPoint(hitWorldPos);
            var w = WoundManager.CreateWound(entity, localPos, hitNormal, damage, bodyPart);

            // spawn impact particles and droplets
            DropletManager.SpawnGore(hitWorldPos, hitNormal, damage * ConfigManager.Settings.BloodMultiplier);

            // track hits for pool spawning conditions. Pools only spawn under corpses after delay.
            BloodPoolManager.TrackHit(entity, bodyPart, damage, hitSource);

            // spawn wall decals if surface
            if (hitSource == null)
            {
                DecalManager.SpawnWallSplat(hitWorldPos, hitNormal, 1.0f + damage * 0.5f, Mathf.Clamp01(damage*0.2f));
            }

            // if hit source is a weapon, add blood to it
            if (hitSource != null)
            {
                WeaponBloodManager.AddBloodToWeapon(hitSource.transform, damage * 0.1f);
            }
        }
    }
}
