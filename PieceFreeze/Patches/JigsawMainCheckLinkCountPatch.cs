using Bnfour.MoeJigsawMods.PieceFreeze.Utilities;
using HarmonyLib;

using Jigsaw;
using Jigsaw.Piece;

namespace Bnfour.MoeJigsawMods.PieceFreeze.Patches;

[HarmonyPatch(typeof(JigsawMain), "checkLinkCount")]
public class JigsawMainCheckLinkCountPatch
{
    // called every piece of the connected build on dock
    internal static void Postfix(Model m)
    {
        FreezeManager.ProcessDocking(m);
    }
}
