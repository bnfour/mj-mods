using HarmonyLib;
using UnityEngine;

namespace Bnfour.MoeJigsawMods.CrisperImages.Patches;

[HarmonyPatch(typeof(Complete), "Update")]
public class CompleteUpdatePatch
{
    internal static void Postfix(GameObject ___srMain)
    {
        if (___srMain?.transform.localScale == Vector3.one)
        {
            var rect = ___srMain.GetComponent<SpriteRenderer>().sprite.rect;
            var scaleX = GameManager.Instance.puzzleMainInfo.puzzleInfo.current.frameW / rect.width;
            var scaleY = GameManager.Instance.puzzleMainInfo.puzzleInfo.current.frameH / rect.height;

            ___srMain.transform.localScale = new(scaleX, scaleY, 1);
        }
    }
}
