using MelonLoader;
using UnityEngine;
using BloodLabMod.Core;

[assembly: MelonInfo(typeof(BloodLabMod.Core.BloodLabMod), "BloodLabMod", "1.0.0", "fxleons")]
namespace BloodLabMod.Core
{
    public class BloodLabMod : MelonMod
    {
        private static bool initialized = false;

        public override void OnApplicationStart()
        {
            if (initialized) return;
            initialized = true;
            MelonLogger.Msg("BloodLabMod initializing...");
            ConfigManager.Load();
            BloodConfig.Initialize();
            BloodMenuIntegration.Initialize();
            HookManager.Initialize();
            BonelabDamageIntegration.Initialize();
        }

        public override void OnUpdate()
        {
            BloodMenuIntegration.Update();
        }
    }
}
