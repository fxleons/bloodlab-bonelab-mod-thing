## Thank you!

Hey! Thank you for downloading BLOODLAB, it's a personal project I made inspired
 by BLOODWORKS. Honestly, I don't know how BONELAB doesn't have any good blood m
ods, I had to make this just out of pure anger for not finding one.

> This version uses the BONELAB vanilla decals, so no prefab setup is required. Don't worry about SDK mods or something like that, since it's already done.
Key features implemented:
- Impact blood particles and physics droplets
- Persistent wounds attached to body parts
- Realistic bleeding over time
- Physics-based droplets that create puddles
- Wall splatters (decals)
- Weapon and player blood tracking
- Object pooling and configurable limits

Let me explain what I did in a simple way.

# Building

Use the generated game DLLs for a real build. Either point directly at the DLLs or set directories:

- `UNITY_DLL="/path/to/UnityEngine.dll"`
- `MELON_DLL="/path/to/MelonLoader.dll"`
- or `UNITY_DIR="/path/to/unity" MELON_DIR="/path/to/melonloader"`

If `MELON_DIR` contains `net6/MelonLoader.dll`, the script will use that automatically.


# The blood
Simple. Get vanilla blood particles and decals, then use them to my advantage. Making the blood decals small, particles turn into droplets, and droplets turn in to little blood drops. Eventually, after certain amount of shots, the NPC (or player) will bleed out and die of blood loss. Blood can splatter on walls, ceilings and floors. Only spawning on ceilings if the NPC is tall enough, or if you're in a situation where you're really close to the ceiling.

# Weapon to blood physics
The weapon does raycast things and makes funny bullets. Well, what I did was catch that raycast and use it to see where to send the blood to.

## PLEASE like the project, it took so long to make that I might have grown a beard while working on it.
programming time took like a month or a bit less btw, mainly cause i tried to make my first code mod which is this after 4 sdk mods
