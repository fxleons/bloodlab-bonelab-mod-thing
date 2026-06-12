using System.Collections.Generic;
using UnityEngine;

namespace BloodLabMod.Core
{
    public static class WeaponBloodManager
    {
        private static Dictionary<Transform, float> weaponBlood = new Dictionary<Transform, float>();

        public static void Initialize() { }

        public static void AddBloodToWeapon(Transform weapon, float amount)
        {
            if (!weaponBlood.ContainsKey(weapon)) weaponBlood[weapon] = 0f;
            weaponBlood[weapon] += amount;
            // optionally spawn small decals on weapon
        }

        public static void UpdateAll(float dt)
        {
            var keys = new List<Transform>(weaponBlood.Keys);
            foreach (var k in keys)
            {
                weaponBlood[k] = Mathf.Max(0f, weaponBlood[k] - dt * 0.001f * ConfigManager.Settings.BloodDryingSpeed);
            }
        }
    }
}
