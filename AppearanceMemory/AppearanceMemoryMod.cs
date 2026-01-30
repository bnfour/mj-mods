using MelonLoader;

using Bnfour.MoeJigsawMods.AppearanceMemory.Validators;

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
        _category = MelonPreferences.CreateCategory("Bnfour_AppearanceMemory");
        _skinId = _category.CreateEntry("Skin", 1, "Skin ID", "Index of the background image to use, 1–8.",
            validator: new SkinIdValidator());
        _trayId = _category.CreateEntry("Tray", 1, "Tray ID", "Index of the tray color to use, 1–5.",
            validator: new TrayIdValidator());
    }
}
