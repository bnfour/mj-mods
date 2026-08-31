using System.Linq;
using HarmonyLib;

using UnityEngine;

using Jigsaw;
using Jigsaw.Piece;

namespace Bnfour.MoeJigsawMods.PieceFreeze.Patches;

/// <summary>
/// Makes puzzle piece hitboxes aware of freeze feature: if a non-frozen one is
/// on top of another frozen one, it is always selected as hit,
/// preventing some frustrating locks.
/// </summary>
/// <remarks>This completely replaces the original method.</remarks>
[HarmonyPatch(typeof(JigsawMain), "GetMouseHitPiece")]
public class JigsawMainGetMouseHitPiecePatch
{
    internal static bool Prefix(ref Model __result, Rect ___trayAllArea)
    {
        // same setup as original method
        var ray = Camera.main.ScreenPointToRay(InputCapture.GetCursorPos());

        // vanilla code is "optimized" by using the greedy raycast that returns
        // the first hit piece regardless of its status,
        // we use a check that returns all hit pieces...
        var hitModels = Physics.RaycastAll(ray)
            .Select(h => h.collider.gameObject.GetComponent<Model>())
            .Where(m => m != null)
            // ...and then sort them, non-frozen first
            .OrderByDescending(x => x, new JigsawMain.PieceSortComparer());

        // TODO this code can undoubtedly be integrated into the above linq
        // but would it be readable?
        foreach (var i in hitModels)
        {
            // as in vanilla, skip pieces on the game field hidden by the tray
            if (i.Place == Model.PLACE.MAIN
                && ___trayAllArea.Contains(InputCapture.GetCursorWorldPos()))
            {
                continue;
            }
            // otherwise, return the first piece from the _sorted_ hit list
            __result = i;
            break;
        }
        // skip the original method completely, we just did its work better than it could
        // it's okay if the result stays null — we just did not hit any pieces
        return false;
    }
}
