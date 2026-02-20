using HarmonyLib;

using Jigsaw;
using MelonLoader;

namespace Bnfour.MoeJigsawMods.DeeperZoom.Patches;

/// <summary>
/// Replaces the maximum zoom scale with a custom one.
/// </summary>
[HarmonyPatch(typeof(JigsawMain), "MakeMainFrame")]
public class JigsawMainMakeMainFramePatch
{
    internal static void Postfix(ref float ___matScaleMax)
    {
        ___matScaleMax = Melon<DeeperZoomMod>.Instance.MatMaxScale;
    }
}
