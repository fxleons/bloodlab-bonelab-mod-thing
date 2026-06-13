using System;
using UnityEngine;

namespace BloodLabMod.Core
{
    public static class BloodConfig
    {
        public const float MinNPCBloodMultiplier = 0f;
        public const float MaxNPCBloodMultiplier = 10f;

        public const float MinDropletSize = 0.01f;
        public const float MaxDropletSize = 2f;

        public const float MinBloodIntensity = 0f;
        public const float MaxBloodIntensity = 5f;

        public static float NPCBloodMultiplier
        {
            get => ConfigManager.Settings.NPCBloodMultiplier;
            set
            {
                ConfigManager.Settings.NPCBloodMultiplier = Mathf.Clamp(value, MinNPCBloodMultiplier, MaxNPCBloodMultiplier);
                Persist();
            }
        }

        public static float DropletSize
        {
            get => ConfigManager.Settings.DropletSize;
            set
            {
                ConfigManager.Settings.DropletSize = Mathf.Clamp(value, MinDropletSize, MaxDropletSize);
                Persist();
            }
        }

        public static float BloodIntensity
        {
            get => ConfigManager.Settings.BloodMultiplier;
            set
            {
                ConfigManager.Settings.BloodMultiplier = Mathf.Clamp(value, MinBloodIntensity, MaxBloodIntensity);
                Persist();
            }
        }

        public static bool EnableDripping
        {
            get => ConfigManager.Settings.EnableDripping;
            set
            {
                ConfigManager.Settings.EnableDripping = value;
                Persist();
            }
        }

        public static bool EnableBloodDecals
        {
            get => ConfigManager.Settings.EnableBloodDecals;
            set
            {
                ConfigManager.Settings.EnableBloodDecals = value;
                Persist();
            }
        }

        public static void Initialize()
        {
            NPCBloodMultiplier = ConfigManager.Settings.NPCBloodMultiplier;
            DropletSize = ConfigManager.Settings.DropletSize;
            BloodIntensity = ConfigManager.Settings.BloodMultiplier;
            EnableDripping = ConfigManager.Settings.EnableDripping;
            EnableBloodDecals = ConfigManager.Settings.EnableBloodDecals;
        }

        private static void Persist()
        {
            ConfigManager.Save();
        }
    }
}
