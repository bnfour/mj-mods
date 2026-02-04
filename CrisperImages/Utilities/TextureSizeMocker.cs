namespace Bnfour.MoeJigsawMods.CrisperImages.Utilities;

public static class TextureSizeMocker
{
    // the arguments are original values we "use" to clear them from the stack

    internal static float OriginalWidth(float _)
    {
        return GameManager.Instance.puzzleMainInfo.puzzleInfo.current.frameW;
    }

    internal static float OriginalHeight(float _)
    {
        return GameManager.Instance.puzzleMainInfo.puzzleInfo.current.frameH;
    }
}
