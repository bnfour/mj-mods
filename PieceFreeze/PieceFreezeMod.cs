using System.Collections.Generic;
using MelonLoader;
using UnityEngine;

namespace Bnfour.MoeJigsawMods.PieceFreeze;

public class PieceFreezeMod : MelonMod
{
    // TODO replace with something that supports storing data for multiple puzzles and file save/load
    // stores piece names as generated in vanilla, piece_XX_YY
    internal HashSet<string> LockedPieces { get; } = new();

    internal bool FreezeModifierDown { get; private set; }

    // TODO make configurable?
    private const KeyCode ModifierKey = KeyCode.LeftAlt;

    public override void OnUpdate()
    {
        FreezeModifierDown = (FreezeModifierDown || Input.GetKeyDown(ModifierKey)) && !Input.GetKeyUp(ModifierKey);
    }
}
