using System.Diagnostics;

using MelonLoader;

namespace Bnfour.MoeJigsawMods.AppearanceMemory.Utilities;

/// <summary>
/// Called when skinID and trayID fields set to save/load values to mod's preferences.
/// </summary>
internal static class FieldPatchHelper
{
    // not skin => tray
    // both methods take original value and custom flag from the stack,
    // return the saved/loaded value to the stack 

    internal static int LoadFromPreferences(int vanillaValue, bool isSkin)
    {
        Debug.Assert(vanillaValue == 1, "unusual vanilla value in Start, did it start to work?");

        var mod = Melon<AppearanceMemoryMod>.Instance;
        return isSkin
            ? mod.Skin
            : mod.Tray;
    }

    internal static int SaveToPreferences(int newValue, bool isSkin)
    {
        var mod = Melon<AppearanceMemoryMod>.Instance;
        if (isSkin)
        {
            mod.Skin = newValue;
        }
        else
        {
            mod.Tray = newValue;
        }

        return newValue;
    }
}
