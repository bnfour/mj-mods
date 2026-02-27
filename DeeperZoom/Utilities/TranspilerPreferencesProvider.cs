using MelonLoader;

namespace Bnfour.MoeJigsawMods.DeeperZoom.Utilities;

/// <summary>
/// Calls to this inserted in transpiler when mod's preferences are not loaded yet.
/// Actually called later, when the preferences are available.
/// </summary>
internal static class TranspilerPreferencesProvider
{
    // static methods for trivial calling

    internal static int ZoomSteps()
    {
        return Melon<DeeperZoomMod>.Instance.ZoomSteps;
    }

    internal static float ZoomStepsMinusOne()
    {
        return Melon<DeeperZoomMod>.Instance.ZoomSteps - 1;
    }

    internal static int PreviewZoomMax()
    {
        return Melon<DeeperZoomMod>.Instance.PreviewZoomMax;
    }

    internal static int PreviewZoomMin()
    {
        return Melon<DeeperZoomMod>.Instance.PreviewZoomMin;
    }
}
