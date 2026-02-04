using System.Collections.Generic;
using System.Reflection.Emit;

using HarmonyLib;

namespace Bnfour.MoeJigsawMods.DeeperZoom.Patches;

[HarmonyPatch(typeof(PreviewWindow), "Update")]
public class PreviewWindowUpdatePatch
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        // zoom for preview window works differently from the main puzzle:
        // here, we have zoom levels 4 to 10, 6 is default (in vanilla)
        // every scroll event changes current value by one within these bounds

        // TODO load from prefs (save caveat as the other transpiler), currently hardcoded to zoom 1–20
        // (probably excessive on both sides)

        foreach (var instruction in instructions)
        {
            if (instruction.opcode == OpCodes.Ldc_I4_4)
            {
                // TODO hardcoded to optimally load one, replace with ldc.i4.s
                yield return new CodeInstruction(OpCodes.Ldc_I4_1);
            }
            else if (instruction.opcode == OpCodes.Ldc_I4_S
                && instruction.operand is sbyte i && i == 10)
            {
                // TODO should probably keep short push, zoom 127 is ought to be enough
                yield return new CodeInstruction(OpCodes.Ldc_I4_S, (sbyte)20);
            }
            else
            {
                yield return instruction;
            }
        }
    }
}
