using System.Linq;
using MelonLoader;

using MelonLoader.Preferences;

namespace Bnfour.MoeJigsawMods.AppearanceMemory;

public class AppearanceMemoryMod : MelonMod
{
    private MelonPreferences_Category _category;
    private MelonPreferences_Entry<int> _skinId;
    private MelonPreferences_Entry<int> _trayId;

    internal int Skin
    {
        get => _skinId.Value;
        set => _skinId.Value = value;
    }

    internal int Tray
    {
        get => _trayId.Value;
        set => _trayId.Value = value;
    }

    public override void OnInitializeMelon()
    {

        var customBgPresent = MelonBase.RegisteredMelons.Any(m => m.Info.Author == "bnfour" && m.Info.Name == "Custom background");
        var skinIdUpperBound = customBgPresent ? 9 : 8;

        _category = MelonPreferences.CreateCategory("Bnfour_AppearanceMemory");
        _skinId = _category.CreateEntry("Skin", 1, "Skin ID", "Index of the background image to use, 1–8. (9 if Custom background is also installed.)",
            validator: new ValueRange<int>(1, skinIdUpperBound));
        _trayId = _category.CreateEntry("Tray", 1, "Tray ID", "Index of the tray color to use, 1–5.",
            validator: new ValueRange<int>(1, 5));
    }
}
