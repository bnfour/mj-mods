using MelonLoader;
using UnityEngine;

using Bnfour.MoeJigsawMods.PieceFreeze.Utilities;

namespace Bnfour.MoeJigsawMods.PieceFreeze;

public class PieceFreezeMod : MelonMod
{
    internal DataStorage LockedData { get; } = new();

    internal bool FreezeModifierDown { get; private set; }

    private const KeyCode ModifierKey = KeyCode.LeftAlt;

    public override void OnInitializeMelon()
    {
        LockedData.Load();
    }

    public override void OnUpdate()
    {
        FreezeModifierDown = UpdateKeydownStatus(FreezeModifierDown, ModifierKey);
    }

    public override void OnApplicationQuit()
    {
        LockedData.Save();
    }

    private bool UpdateKeydownStatus(bool currentValue, KeyCode keyCode)
    {
        return (currentValue || Input.GetKeyDown(keyCode)) && !Input.GetKeyUp(keyCode);
    }
}
