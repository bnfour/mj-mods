using HarmonyLib;

using Jigsaw;

namespace Bnfour.MoeJigsawMods.CustomBackground.Patches;

/// <summary>
/// Adds another entry to the list of defined skin names, using within the code
/// to manage the UI.
/// </summary>
[HarmonyPatch(typeof(JigsawMain), "Start")]
public class SkinNameExtender
{
    internal static void Postfix(ref string[] ___skinNameArray)
    {
        ___skinNameArray = [.. ___skinNameArray, "skin9"];
    }
}
