using System.Linq;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

using Jigsaw;

namespace Bnfour.MoeJigsawMods.DeeperZoom.Patches;

/// <summary>
/// Replaces the maximum zoom scale with a custom one.
/// Changes the zoom level on puzzle open to the closest one to 1.0x, the vanilla value.
/// </summary>
[HarmonyPatch(typeof(JigsawMain), "MakeMainFrame")]
public class JigsawMainMakeMainFramePatch
{
    internal static void Postfix(ref float ___matScaleMax, ref int ___baseMatScale,
        float ___matScaleMin, JigsawMain __instance)
    {
        var mod = Melon<DeeperZoomMod>.Instance;

        ___matScaleMax = mod.MatMaxScale;
        // workaround for CS1628, something like "can't use a ref var in lambdas"
        var matScaleMax = ___matScaleMax;

        ___baseMatScale = Enumerable.Range(1, mod.ZoomSteps)
            // calculate the zoom scale via lerp just like vanilla code
            .Select(x => new LemonTuple<int, float>(x, Mathf.Abs(1f - Mathf.Lerp(___matScaleMin, matScaleMax, (float)(x - 1) / (mod.ZoomSteps - 1)))))
            // and find the level that provides closest possible zoom to vanilla's default of 1.0 (also the vanilla maximum)
            .OrderBy(t => t.Item2)
            .First().Item1;

        // set zoom to the level we found as default
        var traverse = Traverse.Create(__instance);
        traverse.Method("CalcMovableArea").GetValue();
        traverse.Method("AdjustMatPosition").GetValue();
    }
}
