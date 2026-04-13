using HarmonyLib;
using UnityEngine;

using Jigsaw;

using Bnfour.MoeJigsawMods.ZoomToCursor.Data;

namespace Bnfour.MoeJigsawMods.ZoomToCursor.Patches;


[HarmonyPatch(typeof(JigsawMain), "Update")]
internal class JigsawMainUpdatePatch
{
    internal static void Prefix(int ___baseMatScale, GameObject ___goMatBase,
        out ZoomState __state)
    {
        // store the state before the update

        var currentPositionOverObject = ___goMatBase.transform.InverseTransformPoint(
            Camera.main.ScreenToWorldPoint(InputCapture.GetCursorWorldPos()));

        __state = new()
        {
            ZoomLevel = ___baseMatScale,
            PositionOverTheObject = (Vector2)currentPositionOverObject
        };
    }

    internal static void Postfix(JigsawMain __instance,
        int ___baseMatScale, GameObject ___goMatBase, ref Vector2 ___vCamPos,
        ZoomState __state)
    {
        if (___baseMatScale != __state.ZoomLevel)
        {
            // TODO stuff
        }
    }
}
