using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

using Bnfour.MoeJigsawMods.DeeperZoom.Utilities;

namespace Bnfour.MoeJigsawMods.DeeperZoom.Patches;

[HarmonyPatch(typeof(PreviewWindow), "Update")]
public class PreviewWindowUpdatePatch
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        // zoom for preview window works differently from the main puzzle:
        // here, we have zoom levels 4 to 10, 6 is default (in vanilla)
        // every scroll event changes current value by one within these bounds

        foreach (var instruction in instructions)
        {
            if (instruction.opcode == OpCodes.Ldc_I4_4)
            {
                yield return CodeInstruction.Call(typeof(TranspilerPreferencesProvider), nameof(TranspilerPreferencesProvider.PreviewZoomMin));
            }
            else if (instruction.opcode == OpCodes.Ldc_I4_S
                && instruction.operand is sbyte i && i == 10)
            {
                yield return CodeInstruction.Call(typeof(TranspilerPreferencesProvider), nameof(TranspilerPreferencesProvider.PreviewZoomMax));
            }
            else
            {
                yield return instruction;
            }
        }
    }
}
