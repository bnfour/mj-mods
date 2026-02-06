using MelonLoader;
using UnityEngine;

using Bnfour.MoeJigsawMods.PieceFreeze.Utilities;

namespace Bnfour.MoeJigsawMods.PieceFreeze;

public class PieceFreezeMod : MelonMod
{
    internal DataStorage LockedData { get; } = new();

    internal bool FreezeModifierDown { get; private set; }

    // TODO make configurable?
    private const KeyCode ModifierKey = KeyCode.LeftAlt;

    public override void OnInitializeMelon()
    {
        LockedData.Load();
    }

    public override void OnUpdate()
    {
        FreezeModifierDown = (FreezeModifierDown || Input.GetKeyDown(ModifierKey)) && !Input.GetKeyUp(ModifierKey);
    }

    public override void OnApplicationQuit()
    {
        LockedData.Save();
    }
}
