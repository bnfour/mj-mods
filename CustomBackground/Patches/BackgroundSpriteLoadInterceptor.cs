using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using UnityEngine;
using HarmonyLib;

using Jigsaw;

using Bnfour.MoeJigsawMods.CustomBackground.Utilities;

namespace Bnfour.MoeJigsawMods.CustomBackground.Patches;

/// <summary>
/// Inserts a redirect to a custom resource load method instead of plain
/// <see cref="Resources.Load"/> call.
/// </summary>
/// <remarks>
/// The replacement method consumes the string on top of the stack meant for the
///the original call, and pushes a Sprite to it, as the original would.
/// </remarks>
[HarmonyPatch(typeof(JigsawMain), "LoadSkin")]
public class BackgroundSpriteLoadInterceptor
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            // there's only one call to this in the method
            if (instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo mi
                && mi.DeclaringType == typeof(Resources) && mi.Name == nameof(Resources.Load))
            {
                yield return CodeInstruction.Call(typeof(BackgroundSpriteLoadShim),
                    nameof(BackgroundSpriteLoadShim.CustomLoad), [typeof(string)]);
            }
            else
            {
                yield return instruction;
            }
        }
    }
}
