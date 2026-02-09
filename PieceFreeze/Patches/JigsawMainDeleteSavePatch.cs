using HarmonyLib;
using MelonLoader;

using Jigsaw;

namespace Bnfour.MoeJigsawMods.PieceFreeze.Patches;

/// <summary>
/// Removes the lock data for a puzzle when its original save is deleted on completion.
/// </summary>
[HarmonyPatch(typeof(JigsawMain), "DeleteSave")]
public class JigsawMainDeleteSavePatch
{
    internal static void Postfix()
    {
        Melon<PieceFreezeMod>.Instance.LockedData.ResetCurrent();
    }
}
