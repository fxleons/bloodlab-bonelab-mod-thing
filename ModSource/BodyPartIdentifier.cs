using UnityEngine;

namespace BloodLabMod.Core
{
    public static class BodyPartIdentifier
    {
        // Heuristic helper to name hit locations. Replace or extend with
        // BONELAB-specific skeleton/bone names for better results.
        public static string Identify(Transform root, Transform hitTransform)
        {
            if (hitTransform == null) return "unknown";
            var name = hitTransform.name.ToLower();
            if (name.Contains("head") || name.Contains("skull")) return "head";
            if (name.Contains("spine") || name.Contains("chest") || name.Contains("torso")) return "torso";
            if (name.Contains("arm") || name.Contains("hand") || name.Contains("shoulder")) return "arm";
            if (name.Contains("leg") || name.Contains("thigh") || name.Contains("foot")) return "leg";
            return "limb";
        }
    }
}
