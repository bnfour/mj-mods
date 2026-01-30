Unofficial quality of life modifications for the hit video game Moe Jigsaw using MelonLoader.

# Disclaimers
- These mods are unofficial and are not associated with, related to, and/or endorsed by ARES inc.
- USE AT YOUR OWN RISK. NO WARRANTIES.
- Please read [FAQ](#frequently-asked-questions).

# Mod list
The following mods are currently available:
- [Appearance memory](#appearance-memory) — actually saves the background/tray settings

<!-- These mods are all compatible with each other, and can be used in any combination. -->

## Appearance memory
This mod fixes not saving the selection of the background image and the tray color between puzzle screens:

before video

after video

The settings are saved using MelonLoader's preferences framework, inside the default `UserData/MelonPreferences.cfg` file.
Running the game with the mod installed should create the following section in the file:
<!-- TODO commented version -->
```toml
[Bnfour_AppearanceMemory]
Skin = 1
Tray = 1
```

`Skin` can be set to values 1 through 8; `Tray` to 1 through 5 — matching the display order, left to right.
<!-- TODO consider image w/numbers added? -->

# Installation
just copypaste LULE

## Verification
ditto

# Frequently Asked Questions
the same "questions"?

# Building from source
just notice it targets really old stuff; net 3.5 was released in 2007 Aware
