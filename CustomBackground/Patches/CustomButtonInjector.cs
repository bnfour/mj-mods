using System.Linq;
using HarmonyLib;

using ImageLayoutLib;

namespace Bnfour.MoeJigsawMods.CustomBackground.Patches;

/// <summary>
/// Adds a custom button to UI hierarchy tree(?) after it's parsed from resources
/// JSON, but is not yet materialized.
/// </summary>
[HarmonyPatch(typeof(TextureManage), nameof(TextureManage.Load))]
public class CustomButtonInjector
{
    internal static void Postfix(TextureManage.NODE __result, string name)
    {
        // puzzlebtn{0..2} seem to be for various languages
        // only English is really tested, but it should be the same for others
        if (!name.StartsWith("Images/puzzlebtn"))
        {
            return;
        }

        // it's ok to fail if those cannot be found

        var prevKey = __result.Map.Keys.Single(bn => bn.name == "skin8" && bn.type == BLOCKTYPE.PUSHBOX);
        var prevPrevKey = __result.Map.Keys.Single(bn => bn.name == "skin7" && bn.type == BLOCKTYPE.PUSHBOX);

        BlockNode newKey = new() { name = "skin9", type = BLOCKTYPE.PUSHBOX };
        __result.Map[newKey] = new();

        // grab both BlockInfos from previous (to base on) and previous to it (to get offsets from)
        // for each attr value
        // in this particular case, attr2 is completely unused and stays the default value, string.Empty
        foreach (var data in __result.Map[prevKey].Join(__result.Map[prevPrevKey],
            prev => prev.attr1, prevPrev => prevPrev.attr1,
            (prev, prevPrev) => new { prev.attr1, prevBi = prev.bi, prevPrevBi = prevPrev.bi }))
        {
            Attr a = new()
            {
                attr1 = data.attr1,
                attr2 = string.Empty,
                // only x coordinate changes between pushboxes, the rest is copied just in case
                // texture coords not set because we'll use custom sprites instead of vanilla atlas
                bi = new()
                {
                    filename = $"skin9.pushbox.{data.attr1}",
                    id = data.prevBi.id,
                    bx = data.prevBi.bx + (data.prevBi.bx - data.prevPrevBi.bx),
                    by = data.prevBi.by,
                    // ...texture coords skipped...
                    w = data.prevBi.w,
                    h = data.prevBi.h,
                    ox = data.prevBi.ox + (data.prevBi.ox - data.prevPrevBi.ox),
                    oy = data.prevBi.oy,
                    // no idea what priority does, so just copy it
                    pri = data.prevBi.pri
                }
            };

            __result.Map[newKey].Add(a);
        }
    }
}
