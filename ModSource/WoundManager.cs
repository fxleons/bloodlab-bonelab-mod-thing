using System.Collections.Generic;
using UnityEngine;

namespace BloodLabMod.Core
{
    // Manages wounds attached to characters
    public static class WoundManager
    {
        private static List<BloodWound> wounds = new List<BloodWound>();

        public static void Initialize() { }

        public static BloodWound CreateWound(Transform parent, Vector3 localPos, Vector3 normal, float damage, string bodyPart)
        {
            var w = new BloodWound(parent, localPos, normal, damage, bodyPart);
            wounds.Add(w);
            return w;
        }

        public static void UpdateAll(float dt)
        {
            for (int i = wounds.Count - 1; i >= 0; --i)
            {
                var w = wounds[i];
                w.Update(dt);
                if (w.IsFinished)
                    wounds.RemoveAt(i);
            }
        }

        public static IEnumerable<BloodWound> GetWoundsFor(Transform t)
        {
            foreach (var w in wounds)
                if (w.Parent == t) yield return w;
        }
    }
}
