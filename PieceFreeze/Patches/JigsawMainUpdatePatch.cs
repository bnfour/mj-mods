using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using MelonLoader;

using Jigsaw;
using Jigsaw.Piece;

using Bnfour.MoeJigsawMods.PieceFreeze.Utilities;

namespace Bnfour.MoeJigsawMods.PieceFreeze.Patches;

[HarmonyPatch(typeof(JigsawMain), "Update")]
public class JigsawMainUpdatePatch
{
    // this is basically adding a
    // if (FreezeManager.IsFrozen(model, flag)) { model = null; }
    // to the method's code whenever GetMouseHitPiece result is assigned to a local variable
    // it's done twice -- once for drag-and-drop with left mouse button; once for rotating with various inputs
    // the flag value is true only when left mouse button is used

    // pretty straightforward, right? CIL is fun
    // hide_the_pain_harold.jpg

    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilGenerator)
    {
        // we're doing pretty similar thing twice for two method local variables
        // that hold results of GetMouseHitPiece -- their indices are 10 and 12
        // #10 is for drag-and-drop moving
        // #12 is for rotating
        // so we set the data to reuse the rest of the code

        // key is the index
        // bool is "have we seen a ldloc.s instruction for that local yet?" -- we need to create a label on it for custom branching
        // Label is the label to use for branching
        // OpCode is the opcode to push a boolean value for the "is this drag-and-drop" flag
        Dictionary<int, LemonTuple<bool, Label, OpCode>> data = new()
        {
            [10] = new(false, ilGenerator.DefineLabel(), OpCodes.Ldc_I4_1),
            [12] = new(false, ilGenerator.DefineLabel(), OpCodes.Ldc_I4_0)
        };

        foreach (var instruction in instructions)
        {
            // add a label to the first ldloc.s instruction for both locals
            if (instruction.opcode == OpCodes.Ldloc_S
                && instruction.operand is LocalBuilder loadBuilder
                && data.ContainsKey(loadBuilder.LocalIndex)
                && !data[loadBuilder.LocalIndex].Item1)
            {
                instruction.labels.Add(data[loadBuilder.LocalIndex].Item2);
                data[loadBuilder.LocalIndex].Item1 = true;
            }

            yield return instruction;

            // insert the "set local to null if FreezeManager say to do it" code
            // after GetMouseHitPiece calls
            if (instruction.opcode == OpCodes.Stloc_S
                && instruction.operand is LocalBuilder storeBuilder
                && data.ContainsKey(storeBuilder.LocalIndex))
            {
                // push IsFrozen's args to stack
                yield return new CodeInstruction(OpCodes.Ldloc_S, (byte)storeBuilder.LocalIndex);
                yield return new CodeInstruction(data[storeBuilder.LocalIndex].Item3);

                yield return CodeInstruction.Call(typeof(FreezeManager), nameof(FreezeManager.IsFrozen), [typeof(Model), typeof(bool)]);

                // set the local to null (to effectively skip the rest of the method) if IsFrozen returned true
                // it's a bit backwards -- we first put an instruction to skip ahead if a false was returned
                // (that's what the labels are for)
                yield return new CodeInstruction(OpCodes.Brfalse, data[storeBuilder.LocalIndex].Item2);
                // and finally actually set the local to null
                yield return new CodeInstruction(OpCodes.Ldnull);
                yield return new CodeInstruction(OpCodes.Stloc_S, (byte)storeBuilder.LocalIndex);
            }
        }
    }
}
