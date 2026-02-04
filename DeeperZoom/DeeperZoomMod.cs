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
        // TODO finalize validation, esp. max values; descriptions as well
        _maxMatScale = _preferencesCategory.CreateEntry("MaxScale", 2f, validator: new ValueRange<float>(1f, 10f));
        _zoomSteps = _preferencesCategory.CreateEntry("ZoomSteps", 11, validator: new ValueRange<int>(2, 64));
        // match vanilla by default
        _maximumPreviewZoom = _preferencesCategory.CreateEntry("PreviewZoomMax", 10, validator: new ValueRange<int>(2, 50));
        _minimumPreviewZoom = _preferencesCategory.CreateEntry("PreviewZoomMin", 4, validator: new ValueRange<int>(1, 49));
    }
}
