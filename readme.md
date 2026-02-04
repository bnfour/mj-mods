Unofficial quality of life modifications for the hit video game Moe Jigsaw using MelonLoader.

# Disclaimers
- These mods are unofficial and are not associated with, related to, and/or endorsed by ARES inc.
- USE AT YOUR OWN RISK. NO WARRANTIES.
- Please read [FAQ](#frequently-asked-questions).

# Mod list
- [Deeper zoom](#deeper-zoom) — customizable zoom levels for the puzzle and preview image

## Deeper zoom
This mod enables custom zoom levels for the puzzle and preview image.

TODO before/after table? even videos to showcase steps?

### Configuration
The zoom values can be adjusted via mod's preferences. Launching the game with the mod installed should create the following section in MelonLoader's default preferences file, `UserData/MelonPreferences.cfg`:
<!-- TODO commented version -->
```toml
[Bnfour_DeeperZoom]
MaxScale = 2.0
ZoomSteps = 11
PreviewZoomMax = 10
PreviewZoomMin = 4
```

By default, preview image's zoom is unchanged. Adjust if necessary.
<!-- TODO bounds might be adjusted, don;t forget to change here as well -->
Please note that some validation is in place:
- `MaxScale` cannot be lower than 1.0, the vanilla value  
(it also cannot be higher than 10.0, but why would you need _that_ much zoom?)
- `ZoomSteps` cannot be lower than 2 to at least allow switching between minimum and maximum possible scales
- `PreviewZoomMax` cannot be lower than 2 to at least allow two zoom levels  
(and higher than 50 (not that you'll need it))
- `PreviewZoomMin` cannot be lower than 1; it is also forced to be less than `PreviewZoomMax`  
(so yep, no higher than 49)

# Installation
just copypaste LULE

## Verification
ditto

# Frequently Asked Questions
the same "questions"?

# Building from source
just notice it targets really old stuff; net 3.5 was released in 2007 Aware
