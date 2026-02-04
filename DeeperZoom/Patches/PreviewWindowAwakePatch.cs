using HarmonyLib;

namespace Bnfour.MoeJigsawMods.DeeperZoom.Patches;

/// <summary>
/// Makes sure that the initial zoom value for the preview window is within
/// configured bounds.
/// </summary>
[HarmonyPatch(typeof(PreviewWindow), "Awake")]
public class PreviewWindowAwakePatch
{
    internal static void Postfix(ref int ___Scale)
    {
        // TODO if default of 6 is not within the bounds from prefs,
        // set it to the lowest probably
    }
}
