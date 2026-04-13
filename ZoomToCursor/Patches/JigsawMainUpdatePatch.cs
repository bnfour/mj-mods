using HarmonyLib;
using UnityEngine;

using Jigsaw;

using Bnfour.MoeJigsawMods.ZoomToCursor.Data;

namespace Bnfour.MoeJigsawMods.ZoomToCursor.Patches;


[HarmonyPatch(typeof(JigsawMain), "Update")]
internal class JigsawMainUpdatePatch
{
    // it's more of a reused call than a method, so i didn't move it into a helper call as usual
    private static Vector2 MousePositionInObjectCoords(GameObject go)
        => (Vector2)go.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(InputCapture.GetCursorWorldPos()));

    internal static void Prefix(int ___baseMatScale, GameObject ___goMatBase,
        out ZoomState __state)
    {
        // store the state before the update
        __state = new()
        {
            ZoomLevel = ___baseMatScale,
            PositionOverTheObject = MousePositionInObjectCoords(___goMatBase)
        };
    }

    internal static void Postfix(JigsawMain __instance,
        int ___baseMatScale, GameObject ___goMatBase, ref Vector2 ___vCamPos,
        ZoomState __state)
    {
        if (___baseMatScale != __state.ZoomLevel)
        {
            ___vCamPos += MousePositionInObjectCoords(___goMatBase) - __state.PositionOverTheObject;

            // apply camera position change
            Traverse.Create(__instance).Method("AdjustMatPosition").GetValue();
        }
    }
}
