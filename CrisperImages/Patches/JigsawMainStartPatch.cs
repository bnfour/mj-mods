using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using HarmonyLib;

using Jigsaw;

namespace Bnfour.MoeJigsawMods.CrisperImages.Patches;

/// <summary>
/// Replaces the puzzle texture (nameMain) with the higher resolution version
/// originally used by the gallery (nameFull).
/// </summary>
[HarmonyPatch(typeof(JigsawMain), "Start")]
public class JigsawMainStartPatch
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            if (instruction.opcode == OpCodes.Ldfld && instruction.operand is FieldInfo fi
                && fi.Name == "nameMain")
            {
                yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PuzzleInfo.Mode), nameof(PuzzleInfo.Mode.nameFull)));
            }
            else
            {
                yield return instruction;
            }
        }
    }
}
