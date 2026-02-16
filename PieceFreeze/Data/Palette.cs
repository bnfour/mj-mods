using UnityEngine;

namespace Bnfour.MoeJigsawMods.PieceFreeze.Data;

/// <summary>
/// Defines the custom colors for the lock/unlock SFX.
/// </summary>
internal static class Palette
{
    internal static Color Locked => Color.red;
    internal static Color Locking => Color.cyan;
    internal static Color Unlocking => Color.magenta;
}
