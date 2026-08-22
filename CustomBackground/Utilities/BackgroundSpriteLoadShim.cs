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
            // TODO load custom sprite
            return Resources.Load<Sprite>("Images/basesp1");
        }
        return Resources.Load<Sprite>(path);
    }
}
