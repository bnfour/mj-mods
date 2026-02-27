using HarmonyLib;
using MelonLoader;

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
        var mod = Melon<DeeperZoomMod>.Instance;
        if (___Scale < mod.PreviewZoomMin || ___Scale > mod.PreviewZoomMax)
        {
            ___Scale = mod.PreviewZoomMin;
        }
    }
}
