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
            if (entity == null) return;
            ApplyNativeBloodEffects(entity, hitWorldPos, hitNormal, damage, bodyPart, hitSource);
        }

        private static void ApplyNativeBloodEffects(Transform entity, Vector3 hitWorldPos, Vector3 hitNormal, float damage, string bodyPart, GameObject hitSource)
        {
            // Intentionally left empty for BONELAB native integration.
            // Do not spawn custom decals, droplets, puddles, or wounds here.
            // If BONELAB exposes a native blood effect or damage vfx API,
            // it should be invoked from this method.
        }
    }
}
