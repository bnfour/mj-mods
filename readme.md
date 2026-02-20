Unofficial quality of life modifications for the hit video game Moe Jigsaw using MelonLoader.

# Disclaimers
- These mods are unofficial and are not associated with, related to, and/or endorsed by ARES inc.
- USE AT YOUR OWN RISK. NO WARRANTIES.
- Please read [FAQ](#frequently-asked-questions).

# Mod list
- [Piece freeze](#piece-freeze) — allows to lock parts of the puzzle from accidental changes

## Piece freeze
This mod allows to lock pieces in place to prevent accidental interactions. Any pieces newly attached to a locked group will become locked too. Locked pieces will flash red on attempts to move or rotate them.

TODO video(s?)

Click a piece while holding <kbd>Alt</kbd> (either one works) to toggle the lock state for it and all the other pieces connected to it.

### Configuration
It's possible to disable the sound effects that this mod introduced by default — some vanilla sounds are reused for pieces (un)locking and showing their locked state.

The mod's preferences are stored in MelonLoader's default preferences file, `UserData/MelonPreferences.cfg`. Launching the game with the mod installed should create the following section in the file:
```toml
[Bnfour_PieceFreeze]
# Play sounds on pieces (un)locking.
Sounds = true
```

Set the value to `false` to disable the custom sounds.

# Installation
just copypaste LULE

## Verification
ditto

# Frequently Asked Questions
the same "questions"?

# Building from source
just notice it targets really old stuff; net 3.5 was released in 2007 Aware
