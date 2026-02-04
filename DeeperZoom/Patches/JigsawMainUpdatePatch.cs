using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

using Jigsaw;

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
        // TODO load from prefs, at least 2 for min/max, more for in-between steps
        // TODO if i recall correctly, the prefs are not available when transpiler runs
        // if really so, replace with a call to a helper method
        // that runs when prefs are available and just returns the value (to the stack)?
        int zoomSteps = 20;

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
                yield return new CodeInstruction(OpCodes.Ldc_I4_S, (sbyte)zoomSteps);
            }
            // used to get 0–1 ratio to use with lerp
            // (9 is maximum value - 1, so that zoom level 1 is ratio (1 - 1)/(10 - 1) = 0 => smallest scale)
            else if (instruction.opcode == OpCodes.Ldc_R4
                // fuzzy comparison just in case, _seems_ to work just as fine with simple ==
                && instruction.operand is float f && Math.Abs(9f - f) <= 0.01f)
            {
                yield return new CodeInstruction(OpCodes.Ldc_R4, (float)(zoomSteps - 1));
            }
            else
            {
                yield return instruction;
            }
        }
    }
}
