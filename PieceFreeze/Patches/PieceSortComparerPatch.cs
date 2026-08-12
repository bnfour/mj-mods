using HarmonyLib;

using Jigsaw;
using Jigsaw.Piece;

using Bnfour.MoeJigsawMods.PieceFreeze.Utilities;

namespace Bnfour.MoeJigsawMods.PieceFreeze.Patches;

/// <summary>
/// Overrides the puzzle piece z-ordering, deprioritizing locked pieces.
/// </summary>
[HarmonyPatch(typeof(JigsawMain.PieceSortComparer), nameof(JigsawMain.PieceSortComparer.Compare))]
public class PieceSortComparerPatch
{
    internal static bool Prefix(Model m1, Model m2, ref int __result)
    {
        // replace the original logic if only one of two models to compare is frozen

        var m1Frozen = FreezeManager.IsFrozen(m1);
        if (m1Frozen ^ FreezeManager.IsFrozen(m2))
        {
            __result = m1Frozen ? -1 : 1;
            return false;
        }
        return true;
    }
}
