using MelonLoader;
using UnityEngine;

using Bnfour.MoeJigsawMods.PieceFreeze.Utilities;

namespace Bnfour.MoeJigsawMods.PieceFreeze;

public class PieceFreezeMod : MelonMod
{
    internal DataStorage LockedData { get; } = new();

    internal bool FreezeModifierDown => _lAltDown || _rAltDown || _altGrDown;

    internal bool EnableSound => _enableSound.Value;

    private bool _lAltDown;
    private bool _rAltDown;
    private bool _altGrDown;

    private MelonPreferences_Category _preferencesCategory;
    private MelonPreferences_Entry<bool> _enableSound;

    public override void OnInitializeMelon()
    {
        LockedData.Load();

        _preferencesCategory = MelonPreferences.CreateCategory("Bnfour_PieceFreeze");
        _enableSound = _preferencesCategory.CreateEntry("Sounds", true, "Sounds", "Play sounds on pieces (un)locking.");
    }

    public override void OnUpdate()
    {
        _lAltDown = UpdateKeydownStatus(_lAltDown, KeyCode.LeftAlt);
        _rAltDown = UpdateKeydownStatus(_rAltDown, KeyCode.RightAlt);
        _altGrDown = UpdateKeydownStatus(_altGrDown, KeyCode.AltGr);
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
