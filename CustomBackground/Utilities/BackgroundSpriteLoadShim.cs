using MelonLoader;
using UnityEngine;

namespace Bnfour.MoeJigsawMods.CustomBackground.Utilities;

/// <summary>
/// Adds an ability to load a custom background sprite into original code.
/// Implemented as a static method for simple calling.
/// </summary>
public static class BackgroundSpriteLoadShim
{
    internal static Sprite CustomLoad(string path)
    {
        if (path == "Images/basesp9")
        {
            return Melon<CustomBackgroundMod>.Instance.spriteProvider.Background;
        }
        return Resources.Load<Sprite>(path);
    }
}
