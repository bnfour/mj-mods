using MelonLoader;

using Bnfour.MoeJigsawMods.CustomBackground.Utilities;

namespace Bnfour.MoeJigsawMods.CustomBackground;

public class CustomBackgroundMod : MelonMod
{
    internal readonly SpriteProvider spriteProvider = new();

    public override void OnLateInitializeMelon()
    {
        spriteProvider.WarnIfNeeded();
    }
}
