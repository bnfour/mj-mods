using MelonLoader;

namespace Bnfour.MoeJigsawMods.AppearanceMemory;

public class AppearanceMemoryMod : MelonMod
{
    private MelonPreferences_Category _category;
    private MelonPreferences_Entry<int> _skinId;
    private MelonPreferences_Entry<int> _trayId;

    // TODO constraints? 1–8 for skin, 1–5 for tray

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
        // TODO descriptions
        _category = MelonPreferences.CreateCategory("Bnfour_AppearanceMemory");
        _skinId = _category.CreateEntry("Skin", 1);
        _trayId = _category.CreateEntry("Tray", 1);
    }
}
