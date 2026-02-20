using UnityEngine;

namespace Bnfour.MoeJigsawMods.PieceFreeze.Data;

/// <summary>
/// Defines the custom colors for the lock/unlock SFX.
/// </summary>
internal static class Palette
{
    internal static Color Locked => Color.red;
    internal static Color Locking => new(1f, .175f, 0f, 1f);
    internal static Color Unlocking => Color.green;
}
