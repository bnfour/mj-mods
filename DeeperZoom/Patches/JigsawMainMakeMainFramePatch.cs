using HarmonyLib;

using Jigsaw;

namespace Bnfour.MoeJigsawMods.DeeperZoom.Patches;

/// <summary>
/// Replaces the maximum zoom scale with a custom one.
/// </summary>
[HarmonyPatch(typeof(JigsawMain), "MakeMainFrame")]
public class JigsawMainMakeMainFramePatch
{
    internal static void Postfix(ref float ___matScaleMax)
    {
        // TODO load from preferences, at least 1 to match vanilla
        var newMatScaleMax = 2f;
        ___matScaleMax = newMatScaleMax;
    }
}
