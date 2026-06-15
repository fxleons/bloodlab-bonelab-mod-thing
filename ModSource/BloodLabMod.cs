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
            PoolManager.Initialize();
            BloodPoolManager.Initialize();
            WoundManager.Initialize();
            DropletManager.Initialize();
            PuddleManager.Initialize();
            DecalManager.Initialize();
            WeaponBloodManager.Initialize();
            PlayerBloodManager.Initialize();
            HookManager.Initialize();
            BonelabDamageIntegration.Initialize();
            RagdollBloodSystem.Initialize();
        }

        public override void OnUpdate()
        {
            BloodMenuIntegration.Update();
            WoundManager.UpdateAll(Time.deltaTime);
            DropletManager.UpdateAll(Time.deltaTime);
            PuddleManager.UpdateAll(Time.deltaTime);
            BloodPoolManager.UpdateAll(Time.deltaTime);
            WeaponBloodManager.UpdateAll(Time.deltaTime);
            PlayerBloodManager.UpdateAll(Time.deltaTime);
        }
    }
}
