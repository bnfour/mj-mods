using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

using Jigsaw.Piece;

using Bnfour.MoeJigsawMods.CrisperImages.Utilities;

namespace Bnfour.MoeJigsawMods.CrisperImages.Patches;

/// <summary>
/// Replaces sizes of the actual texture (now bigger than original) with original
/// values, for the UV calculation (that thinks texture dimensions match piece grid),
/// so it still results in full texture area being used.
/// The UV is dimensionless -- we just need the code to output the (0,0) to (1,1) square.
/// </summary>
[HarmonyPatch(typeof(Model), nameof(Model.Create))]
public class ModelCreatePatch
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        // texture width and height are 2nd and 3rd locals of the method respectively
        foreach (var instruction in instructions)
        {
            if (instruction.opcode == OpCodes.Stloc_2)
            {
                yield return CodeInstruction.Call(typeof(TextureSizeMocker), nameof(TextureSizeMocker.OriginalWidth), [typeof(float)]);
            }
            else if (instruction.opcode == OpCodes.Stloc_3)
            {
                yield return CodeInstruction.Call(typeof(TextureSizeMocker), nameof(TextureSizeMocker.OriginalHeight), [typeof(float)]);
            }
            yield return instruction;
        }
    }
}
