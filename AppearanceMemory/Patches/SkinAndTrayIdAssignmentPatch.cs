using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

using Jigsaw;

using Bnfour.MoeJigsawMods.AppearanceMemory.Utilities;

namespace Bnfour.MoeJigsawMods.AppearanceMemory.Patches;

[HarmonyPatch]
public class SkinAndTrayIdAssignmentPatch
{
    internal static IEnumerable<MethodBase> TargetMethods()
    {
        // private method
        yield return AccessTools.Method(typeof(JigsawMain), "Start");
        yield return AccessTools.Method(typeof(JigsawMain), nameof(JigsawMain.OnAction));
    }

    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase __originalMethod)
    {
        foreach (var instruction in instructions)
        {
            // check if the next instruction is assignment to either skinID or trayID,
            // and if so, run the custom logic depending on the method we're in:
            // - load from preferences in Start, discarding the original value (seems to be 1 always)
            // - save to preferences in OnAction, passing the new value to the setter
            if (instruction.opcode == OpCodes.Stfld)
            {
                var operand = instruction.operand as FieldInfo;
                if (operand?.DeclaringType == typeof(JigsawMain) && operand?.FieldType == typeof(int))
                {
                    bool? isSkin = operand?.Name switch
                    {
                        "skinID" => true,
                        "trayID" => false,
                        _ => null
                    };

                    if (isSkin.HasValue)
                    {
                        var methodName = __originalMethod?.Name switch
                        {
                            "Start" => nameof(FieldPatchHelper.LoadFromPreferences),
                            "OnAction" => nameof(FieldPatchHelper.SaveToPreferences),
                            _ => null
                        };
                        if (methodName != null)
                        {
                            // push isSkin flag to the stack; apparently bools are i4 internally
                            yield return new CodeInstruction(isSkin.Value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
                            yield return CodeInstruction.Call(typeof(FieldPatchHelper), methodName, [typeof(int), typeof(bool)]);
                        }
                    }
                }
            }
            // always execute the original instruction
            yield return instruction;
        }
    }
}
