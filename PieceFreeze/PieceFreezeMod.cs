using MelonLoader;
using UnityEngine;

using Bnfour.MoeJigsawMods.PieceFreeze.Utilities;

namespace Bnfour.MoeJigsawMods.PieceFreeze;

public class PieceFreezeMod : MelonMod
{
    internal DataStorage LockedData { get; } = new();

    internal bool FreezeModifierDown => _lAltDown || _rAltDown || _altGrDown;

    private bool _lAltDown;
    private bool _rAltDown;
    private bool _altGrDown;

    public override void OnInitializeMelon()
    {
        LockedData.Load();
    }

    public override void OnUpdate()
    {
        _lAltDown = UpdateKeydownStatus(_lAltDown, KeyCode.LeftAlt);
        _rAltDown = UpdateKeydownStatus(_lAltDown, KeyCode.RightAlt);
        _altGrDown = UpdateKeydownStatus(_lAltDown, KeyCode.AltGr);
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
