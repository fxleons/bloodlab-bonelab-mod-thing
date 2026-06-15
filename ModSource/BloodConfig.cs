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

        public static bool EnableBloodPools
        {
            get => ConfigManager.Settings.EnableBloodPools;
            set
            {
                ConfigManager.Settings.EnableBloodPools = value;
                Persist();
            }
        }

        public static float PoolDelay
        {
            get => ConfigManager.Settings.PoolDelay;
            set
            {
                ConfigManager.Settings.PoolDelay = Mathf.Clamp(value, MinPoolDelay, MaxPoolDelay);
                Persist();
            }
        }

        public static float PoolGrowthSpeed
        {
            get => ConfigManager.Settings.PoolGrowthSpeed;
            set
            {
                ConfigManager.Settings.PoolGrowthSpeed = Mathf.Clamp(value, MinPoolGrowthSpeed, MaxPoolGrowthSpeed);
                Persist();
            }
        }

        public static float PoolMaxSize
        {
            get => ConfigManager.Settings.PoolMaxSize;
            set
            {
                ConfigManager.Settings.PoolMaxSize = Mathf.Clamp(value, MinPoolMaxSize, MaxPoolMaxSize);
                Persist();
            }
        }

        public static int MaxActivePools
        {
            get => ConfigManager.Settings.MaxActivePools;
            set
            {
                ConfigManager.Settings.MaxActivePools = Mathf.Clamp(value, MinMaxActivePools, MaxMaxActivePools);
                Persist();
            }
        }

        public static bool HeadshotsOnly
        {
            get => ConfigManager.Settings.HeadshotsOnly;
            set
            {
                ConfigManager.Settings.HeadshotsOnly = value;
                Persist();
            }
        }

        public const float MinPoolDelay = 0f;
        public const float MaxPoolDelay = 120f;
        public const float MinPoolGrowthSpeed = 0.01f;
        public const float MaxPoolGrowthSpeed = 1f;
        public const float MinPoolMaxSize = 0.1f;
        public const float MaxPoolMaxSize = 5f;
        public const int MinMaxActivePools = 1;
        public const int MaxMaxActivePools = 50;

        public static void Initialize()
        {
            NPCBloodMultiplier = ConfigManager.Settings.NPCBloodMultiplier;
            DropletSize = ConfigManager.Settings.DropletSize;
            BloodIntensity = ConfigManager.Settings.BloodMultiplier;
            EnableDripping = ConfigManager.Settings.EnableDripping;
            EnableBloodDecals = ConfigManager.Settings.EnableBloodDecals;
            EnableBloodPools = ConfigManager.Settings.EnableBloodPools;
            PoolDelay = ConfigManager.Settings.PoolDelay;
            PoolGrowthSpeed = ConfigManager.Settings.PoolGrowthSpeed;
            PoolMaxSize = ConfigManager.Settings.PoolMaxSize;
            MaxActivePools = ConfigManager.Settings.MaxActivePools;
            HeadshotsOnly = ConfigManager.Settings.HeadshotsOnly;
        }

        private static void Persist()
        {
            ConfigManager.Save();
        }
    }
}
