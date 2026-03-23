using System;
using MelonLoader;
using MelonLoader.Preferences;

namespace Bnfour.MoeJigsawMods.DeeperZoom;

public class DeeperZoomMod : MelonMod
{
    private MelonPreferences_Category _preferencesCategory;
    private MelonPreferences_Entry<float> _maxMatScale;
    private MelonPreferences_Entry<int> _zoomSteps;
    private MelonPreferences_Entry<int> _maximumPreviewZoom;
    private MelonPreferences_Entry<int> _minimumPreviewZoom;

    internal float MatMaxScale => _maxMatScale.Value;
    internal int ZoomSteps => _zoomSteps.Value;
    internal int PreviewZoomMax => _maximumPreviewZoom.Value;
    // additional validation so we always have at least two steps
    internal int PreviewZoomMin => Math.Min(_minimumPreviewZoom.Value, PreviewZoomMax - 1);

    public override void OnInitializeMelon()
    {
        _preferencesCategory = MelonPreferences.CreateCategory("Bnfour_DeeperZoom");
        _maxMatScale = _preferencesCategory.CreateEntry("MaxScale", 2f, "Maximum zoom",
        "Maximum zoom value for the puzzle. 1–10 (float), vanilla default is 1.", validator: new ValueRange<float>(1f, 10f));
        _zoomSteps = _preferencesCategory.CreateEntry("ZoomSteps", 11, "Zoom steps",
        "Number of zoom level between minimum and maximum. Inclusive, so can't be less than 2 (max value is 64).", validator: new ValueRange<int>(2, 64));
        // match vanilla by default
        _maximumPreviewZoom = _preferencesCategory.CreateEntry("PreviewZoomMax", 10, "Preview's maximum zoom",
        "Maximum zoom value for the preview image. 2–50 (int), vanilla default is 10 (1x). Measured in 10% increments of original texture size.", validator: new ValueRange<int>(2, 50));
        _minimumPreviewZoom = _preferencesCategory.CreateEntry("PreviewZoomMin", 4, "Preview's maximum zoom",
        "Minimum zoom value for the preview image. 1–49 (int), vanilla default is 4 (0.4x). Should be less than maximum value.", validator: new ValueRange<int>(1, 49));
    }
}
