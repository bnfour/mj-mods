using HarmonyLib;
using UnityEngine;

namespace Bnfour.MoeJigsawMods.CrisperImages.Patches;

/// <summary>
/// Replaces the original preview texture (thumbnail) with a higher resolution 
/// version originally used for the puzzle itself.
/// </summary>
[HarmonyPatch(typeof(PreviewWindow), nameof(PreviewWindow.AttachTexture))]
public class PreviewWindowAttachTexturePatch
{
    internal static void Prefix(ref Texture2D tex)
    {
        // we're loading it separately, because loading of this texture earlier
        // in JigsawMain.Start is overridden in another patch
        tex = GameManager.Instance.puzzleMainInfo.assetBundle
            .LoadAsset<Texture2D>(GameManager.Instance.puzzleMainInfo.puzzleInfo.current.nameMain);
    }
}
