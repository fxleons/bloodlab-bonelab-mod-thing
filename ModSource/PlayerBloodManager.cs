using System.Collections.Generic;
using UnityEngine;

namespace BloodLabMod.Core
{
    public static class PlayerBloodManager
    {
        private static Dictionary<Transform, List<BloodWound>> playerWounds = new Dictionary<Transform, List<BloodWound>>();

        public static void Initialize() { }

        public static void RegisterPlayer(Transform player)
        {
            if (!playerWounds.ContainsKey(player)) playerWounds[player] = new List<BloodWound>();
        }

        public static void AddWound(Transform player, Vector3 localPos, Vector3 normal, float damage, string bodyPart)
        {
            var w = WoundManager.CreateWound(player, localPos, normal, damage, bodyPart);
            if (!playerWounds.ContainsKey(player)) playerWounds[player] = new List<BloodWound>();
            playerWounds[player].Add(w);
        }

        public static void UpdateAll(float dt) { }
    }
}
