using HarmonyLib;
using MelonLoader;

using Jigsaw;

namespace Bnfour.MoeJigsawMods.PieceFreeze.Patches;

[HarmonyPatch(typeof(JigsawMain), "Start")]
public class JigsawMainStartPatch
{
    internal static void Prefix()
    {
        if (GameManager.Instance.puzzleMainInfo.resetPuzzle)
        {
            Melon<PieceFreezeMod>.Instance.LockedData.ResetCurrent();
        }
    }
}
