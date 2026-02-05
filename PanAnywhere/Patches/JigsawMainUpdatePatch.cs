using HarmonyLib;
using MelonLoader;
using UnityEngine;

using Jigsaw;

namespace Bnfour.MoeJigsawMods.PanAnywhere.Patches;

/// <summary>
/// Extra logic for the update loop to enable pan on mouse wheel press anywhere
/// on the screen. Does not support the input replay feature (not that I ever encountered it).
/// </summary>
[HarmonyPatch(typeof(JigsawMain), "Update")]
public class JigsawMainUpdatePatch
{
    private const int WheelButtonCode = 2;

    internal static void Postfix(JigsawMain __instance, ref Vector2 ___vCamPos, GameObject ___goMatBase)
    {
        var mod = Melon<PanAnywhereMod>.Instance;
        if (Input.GetMouseButtonDown(WheelButtonCode))
        {
            mod.DragPosition = InputCapture.GetCursorWorldPos();
        }
        else if (Input.GetMouseButtonUp(WheelButtonCode))
        {
            mod.DragPosition = null;
        }
        else if (mod.DragPosition.HasValue)
        {
            var scale = 1f / ___goMatBase.transform.localScale.x;
            ___vCamPos += scale * (InputCapture.GetCursorWorldPos() - mod.DragPosition.Value);

            Traverse.Create(__instance).Method("AdjustMatPosition").GetValue();
            
            mod.DragPosition = InputCapture.GetCursorWorldPos();
        }
    }
}
