using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

using Jigsaw;

using Bnfour.MoeJigsawMods.DeeperZoom.Utilities;

namespace Bnfour.MoeJigsawMods.DeeperZoom.Patches;

/// <summary>
/// Replaces the number of interpolated zoom steps between minimum and maximum scales
/// with a custom value.
/// </summary>
[HarmonyPatch(typeof(JigsawMain), "Update")]
public class JigsawMainUpdatePatch
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        // the vanilla zoom steps are labeled 1–10,
        // the actual zoom level is calculated by providing (currentStep - 1)/(maxStep - 1)
        // to lerp between minimum and maximum scales set elsewhere

        foreach (var instruction in instructions)
        {
            // used to check that the current zoom level does not exceed the maximum value
            // (10 in vanilla)
            if (instruction.opcode == OpCodes.Ldc_I4_S
                && instruction.operand is sbyte i && i == 10)
            {
                yield return CodeInstruction.Call(typeof(TranspilerPreferencesProvider), nameof(TranspilerPreferencesProvider.ZoomSteps));
            }
            // used to get 0–1 ratio to use with lerp
            // (9 is maximum value - 1, so that zoom level 1 is ratio (1 - 1)/(10 - 1) = 0 => smallest scale)
            else if (instruction.opcode == OpCodes.Ldc_R4
                // fuzzy comparison just in case, _seems_ to work just as fine with simple ==, but better safe than sorry
                && instruction.operand is float f && Math.Abs(9f - f) <= 0.01f)
            {
                yield return CodeInstruction.Call(typeof(TranspilerPreferencesProvider), nameof(TranspilerPreferencesProvider.ZoomStepsMinusOne));
            }
            else
            {
                yield return instruction;
            }
        }
    }
}
