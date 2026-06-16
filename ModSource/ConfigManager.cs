using System;
using System.Globalization;
using System.IO;
using System.Text;
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
            public float PoolDelay = 10f;
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
                    Settings = DeserializeConfig(File.ReadAllText(configPath));
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
                File.WriteAllText(configPath, SerializeConfig(Settings));
            }
            catch
            {
                // ignore save failures so the mod can still run
            }
        }

        private static string SerializeConfig(Config settings)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"BloodMultiplier={settings.BloodMultiplier.ToString(CultureInfo.InvariantCulture)}");
            builder.AppendLine($"PlayerBloodMultiplier={settings.PlayerBloodMultiplier.ToString(CultureInfo.InvariantCulture)}");
            builder.AppendLine($"NPCBloodMultiplier={settings.NPCBloodMultiplier.ToString(CultureInfo.InvariantCulture)}");
            builder.AppendLine($"DropletSize={settings.DropletSize.ToString(CultureInfo.InvariantCulture)}");
            builder.AppendLine($"EnableDripping={settings.EnableDripping}");
            builder.AppendLine($"EnableBloodDecals={settings.EnableBloodDecals}");
            builder.AppendLine($"EnableBloodPools={settings.EnableBloodPools}");
            builder.AppendLine($"PoolDelay={settings.PoolDelay.ToString(CultureInfo.InvariantCulture)}");
            builder.AppendLine($"PoolGrowthSpeed={settings.PoolGrowthSpeed.ToString(CultureInfo.InvariantCulture)}");
            builder.AppendLine($"PoolMaxSize={settings.PoolMaxSize.ToString(CultureInfo.InvariantCulture)}");
            builder.AppendLine($"MaxActivePools={settings.MaxActivePools}");
            builder.AppendLine($"HeadshotsOnly={settings.HeadshotsOnly}");
            builder.AppendLine($"BleedingDurationMultiplier={settings.BleedingDurationMultiplier.ToString(CultureInfo.InvariantCulture)}");
            builder.AppendLine($"MaxDroplets={settings.MaxDroplets}");
            builder.AppendLine($"MaxPuddles={settings.MaxPuddles}");
            builder.AppendLine($"PerformanceMode={settings.PerformanceMode}");
            builder.AppendLine($"DecalLifetime={settings.DecalLifetime.ToString(CultureInfo.InvariantCulture)}");
            builder.AppendLine($"BloodColor={settings.BloodColor.r.ToString(CultureInfo.InvariantCulture)},{settings.BloodColor.g.ToString(CultureInfo.InvariantCulture)},{settings.BloodColor.b.ToString(CultureInfo.InvariantCulture)},{settings.BloodColor.a.ToString(CultureInfo.InvariantCulture)}");
            builder.AppendLine($"DripFrequency={settings.DripFrequency.ToString(CultureInfo.InvariantCulture)}");
            builder.AppendLine($"BloodDryingSpeed={settings.BloodDryingSpeed.ToString(CultureInfo.InvariantCulture)}");
            return builder.ToString();
        }

        private static Config DeserializeConfig(string content)
        {
            var config = new Config();
            var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var parts = line.Split(new[] { '=' }, 2);
                if (parts.Length != 2)
                    continue;

                var key = parts[0].Trim();
                var value = parts[1].Trim();

                switch (key)
                {
                    case "BloodMultiplier":
                        config.BloodMultiplier = TryParseFloat(value, config.BloodMultiplier);
                        break;
                    case "PlayerBloodMultiplier":
                        config.PlayerBloodMultiplier = TryParseFloat(value, config.PlayerBloodMultiplier);
                        break;
                    case "NPCBloodMultiplier":
                        config.NPCBloodMultiplier = TryParseFloat(value, config.NPCBloodMultiplier);
                        break;
                    case "DropletSize":
                        config.DropletSize = TryParseFloat(value, config.DropletSize);
                        break;
                    case "EnableDripping":
                        config.EnableDripping = TryParseBool(value, config.EnableDripping);
                        break;
                    case "EnableBloodDecals":
                        config.EnableBloodDecals = TryParseBool(value, config.EnableBloodDecals);
                        break;
                    case "EnableBloodPools":
                        config.EnableBloodPools = TryParseBool(value, config.EnableBloodPools);
                        break;
                    case "PoolDelay":
                        config.PoolDelay = TryParseFloat(value, config.PoolDelay);
                        break;
                    case "PoolGrowthSpeed":
                        config.PoolGrowthSpeed = TryParseFloat(value, config.PoolGrowthSpeed);
                        break;
                    case "PoolMaxSize":
                        config.PoolMaxSize = TryParseFloat(value, config.PoolMaxSize);
                        break;
                    case "MaxActivePools":
                        config.MaxActivePools = TryParseInt(value, config.MaxActivePools);
                        break;
                    case "HeadshotsOnly":
                        config.HeadshotsOnly = TryParseBool(value, config.HeadshotsOnly);
                        break;
                    case "BleedingDurationMultiplier":
                        config.BleedingDurationMultiplier = TryParseFloat(value, config.BleedingDurationMultiplier);
                        break;
                    case "MaxDroplets":
                        config.MaxDroplets = TryParseInt(value, config.MaxDroplets);
                        break;
                    case "MaxPuddles":
                        config.MaxPuddles = TryParseInt(value, config.MaxPuddles);
                        break;
                    case "PerformanceMode":
                        config.PerformanceMode = TryParseBool(value, config.PerformanceMode);
                        break;
                    case "DecalLifetime":
                        config.DecalLifetime = TryParseFloat(value, config.DecalLifetime);
                        break;
                    case "BloodColor":
                        config.BloodColor = ParseColor(value, config.BloodColor);
                        break;
                    case "DripFrequency":
                        config.DripFrequency = TryParseFloat(value, config.DripFrequency);
                        break;
                    case "BloodDryingSpeed":
                        config.BloodDryingSpeed = TryParseFloat(value, config.BloodDryingSpeed);
                        break;
                }
            }

            return config;
        }

        private static float TryParseFloat(string value, float fallback)
        {
            return float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result) ? result : fallback;
        }

        private static int TryParseInt(string value, int fallback)
        {
            return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) ? result : fallback;
        }

        private static bool TryParseBool(string value, bool fallback)
        {
            return bool.TryParse(value, out var result) ? result : fallback;
        }

        private static Color ParseColor(string value, Color fallback)
        {
            var parts = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 4)
                return fallback;

            var r = TryParseFloat(parts[0].Trim(), fallback.r);
            var g = TryParseFloat(parts[1].Trim(), fallback.g);
            var b = TryParseFloat(parts[2].Trim(), fallback.b);
            var a = TryParseFloat(parts[3].Trim(), fallback.a);
            return new Color(r, g, b, a);
        }
    }
}
