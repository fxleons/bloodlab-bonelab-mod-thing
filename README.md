# BloodLab BONELAB Mod

## Quick install

1. Build `build/BloodLabMod.csproj` in `Release`.
2. Copy `build/build_out/net6.0/Bloodworks_RagdollCompatible_Bonelab.dll` into BONELAB's `Mods/` folder.
3. Launch BONELAB with MelonLoader installed.

> This version generates its blood decals, droplets, and puddles at runtime, so no prefab setup is required.

This repository contains a MelonLoader-compatible BONELAB mod implementing a realistic blood and bleeding system.

Files are in `ModSource/` and are ready to be compiled into a Unity/MelonLoader mod DLL.

Key features implemented in code:
- Impact blood particles and physics droplets
- Persistent wounds attached to body parts
- Realistic bleeding over time
- Physics-based droplets that create puddles
- Wall splatters (decals)
- Weapon and player blood tracking
- Object pooling and configurable limits

Build & MelonLoader setup:
1. Create a Unity project (2020.3 LTS or compatible with BONELAB modding workflow).
2. Copy `ModSource/*.cs` into an `Assembly Definition` or compile into a DLL using a C# project targeting the game's runtime.
3. Add MelonLoader and required references (UnityEngine.dll, UnityEngine.CoreModule.dll, etc.) to the build.
4. Build the project in `Release`.
5. Place the produced DLL into BONELAB’s MelonLoader `Mods/` folder.

Plug-and-play notes:
- The current mod version generates blood decals, droplets, and puddles at runtime.
- No external `BloodPrefabs/` resources are required.
- The final DLL output is `build/build_out/net6.0/Bloodworks_RagdollCompatible_Bonelab.dll`.

Compatibility:
- Works alongside an existing ragdoll mod because it only detects impacts and triggers blood effects.
- It does not modify ragdoll joints, physics, or transforms.

Integration notes:
- `HookManager.OnEntityHit(...)` is the central integration point. Call it from BONELAB's damage handlers for bullets, melee, explosions, and environmental damage.
- Use `BodyPartIdentifier.Identify(root, hitTransform)` to guess a body part.
- For player bleeding, call `PlayerBloodManager.AddWound(playerTransform, localPos, normal, damage, bodyPart)`.

Configuration:
- A `BloodLabModConfig.json` is created in the game's data path on first run. Edit it to adjust multipliers and performance limits.

License: Use and adapt freely for personal modding. Credit is appreciated.