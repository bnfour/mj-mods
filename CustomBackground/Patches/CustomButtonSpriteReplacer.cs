using System.Collections.Generic;

using HarmonyLib;
using MelonLoader;
using UnityEngine;

using ImageLayoutLib;
using ImageLayoutLib.Object;
using Jigsaw;

namespace Bnfour.MoeJigsawMods.CustomBackground.Patches;

/// <summary>
/// Replaces the default custom button sprites (some undefined part of original atlas)
/// with the custom images.
/// </summary>
[HarmonyPatch(typeof(JigsawMain), "LoadButtons")]
public class CustomButtonSpriteReplacer
{
    internal static void Postfix(ImageLayout ___imageLayoutBtn)
    {
        var custom = ___imageLayoutBtn.FindObject("skin9") as ObjPushBox;
        if (custom != null)
        {
            var provider = Melon<CustomBackgroundMod>.Instance.spriteProvider;

            var map = Traverse.Create(custom).Field("Map").GetValue<Dictionary<string, IILObject>>();
            foreach (var kvp in map)
            {
                var a = kvp.Value.gameObject.GetComponent<SpriteRenderer>();
                // possible keys are N, H, NP, HP:
                // N stands for "normal", P stands for "pressed"
                // no idea what H represent, just apply the same sprites to it
                // weskeru: H
                a.sprite = kvp.Key.EndsWith("P") ? provider.ButtonPressed : provider.ButtonNormal;
            }
        }
    }
}
