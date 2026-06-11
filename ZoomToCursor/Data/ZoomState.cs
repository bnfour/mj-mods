using UnityEngine;

namespace Bnfour.MoeJigsawMods.ZoomToCursor.Data;

/// <summary>
/// Used to store and compare some of the game state before/after Update calls.
/// </summary>
public readonly struct ZoomState
{
    /// <summary>
    /// Current zoom level as ordinal; not the scale.
    /// Used to check if zoom in or out was performed this update.
    /// </summary>
    public int ZoomLevel { get; init; }
    /// <summary>
    /// Coordinates of the mouse cursor relative to _the_ puzzle game object.
    /// If zoom was performed, we need to move the camera to restore this value.
    /// </summary>
    public Vector2 PositionOverTheObject { get; init; }
}
