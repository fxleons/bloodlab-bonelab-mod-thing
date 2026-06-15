using System.IO;
using UnityEngine;

namespace BloodLabMod.Core
{
    public static class ConfigManager
    {
        public class Config
        {
            public float BloodMultiplier = 1.0f;
            public float PlayerBloodMultiplier = 1.0f;
            public float NPCBloodMultiplier = 1.0f;
            public float DropletSize = 1.0f;
            public bool EnableDripping = true;
            public bool EnableBloodDecals = true;
            public bool EnableBloodPools = true;
            public float PoolDelay = 30f;
            public float PoolGrowthSpeed = 0.1f;
            public float PoolMaxSize = 1.5f;
            public int MaxActivePools = 12;
            public bool HeadshotsOnly = false;
            public float BleedingDurationMultiplier = 1.0f;
            public int MaxDroplets = 500;
            public int MaxPuddles = 200;
            public bool PerformanceMode = false;
            public float DecalLifetime = 300f;
            public Color BloodColor = new Color(0.6f, 0.05f, 0.03f, 1f);
            public float DripFrequency = 1.0f;
            public float BloodDryingSpeed = 0.01f;
        }

        public static Config Settings = new Config();

        private static string configPath = Path.Combine(UnityEngine.Application.dataPath, "BloodLabModConfig.json");

        public static void Load()
        {
            if (File.Exists(configPath))
            {
                try
                {
                    var json = File.ReadAllText(configPath);
                    Settings = JsonUtility.FromJson<Config>(json) ?? new Config();
                }
                catch
                {
                    Settings = new Config();
                }
            }
            else
            {
                Save();
            }
        }

        public static void Save()
        {
            try
            {
                var json = JsonUtility.ToJson(Settings, true);
                File.WriteAllText(configPath, json);
            }
            catch { }
        }
    }
}
