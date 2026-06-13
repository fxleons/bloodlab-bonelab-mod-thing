# BLOODLAB

## Thank you!

Hey! Thank you for downloading BLOODLAB, it's a personal project I made inspired by BLOODWORKS. Honestly, I don't know how BONELAB doesn't have any good blood mods, I had to make this just out of pure anger for not finding one.

> This version generates its blood decals, droplets, and puddles at runtime, so no prefab setup is required. Don't worry about SDK mods or something like that, since it's already done.

Key features implemented:
- Impact blood particles and physics droplets
- Persistent wounds attached to body parts
- Realistic bleeding over time
- Physics-based droplets that create puddles
- Wall splatters (decals)
- Weapon and player blood tracking
- Object pooling and configurable limits

Let me explain what I did in a simple way.

# The blood
Simple. Get vanilla blood particles and decals, then use them to my advantage. Making the blood decals small, particles turn into droplets, and droplets turn into little blood drops.
Eventually, after certain amount of shots, the NPC (or player) will bleed out and die of blood loss.
Blood can splatter on walls, ceilings and floors. Only spawning on ceilings if the NPC is tall enough, or if you're in a situation where you're really close to the ceiling.

# Weapon to blood physics
The weapon does raycast things and makes funny bullets. Well, what I did was catch that raycast and bullet thing and use it to see where to send the blood to. Easy as that.

## PLEASE like the project, it took so long to make that I might have grown a beard while working on it.
-# programming time took like a month or so btw
