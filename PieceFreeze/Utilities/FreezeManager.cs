using System;
using System.Collections.Generic;

using Jigsaw.Piece;

namespace Bnfour.MoeJigsawMods.PieceFreeze.Utilities;

// this class is only static to simplify calls to it in transpiler
// (i'd rather prefer to emit less CIL by hand)

internal static class FreezeManager
{
    /// <summary>
    /// Checks if the piece user tries to interact with is frozen.
    /// </summary>
    /// <param name="m">Model of the piece user tried to grab or rotate.</param>
    /// <param name="isLeftMouseButton">Whether the left mouse button was used for grabbing
    /// (as opposed to right mouse button or A/D/arrows for rotating).
    /// Used to toggle the state on left-clicking with the modifier held.</param>
    /// <returns></returns>
    internal static bool IsFrozen(Model m, bool isLeftMouseButton)
    {
        // do nothing for now
        return false;
    }

    private static void Lock(HashSet<string> lockedPieces, Model m)
    {
        throw new NotImplementedException("soon™");
    }

    private static void Unlock(HashSet<string> lockedPieces, Model m)
    {
        throw new NotImplementedException("soon™");
    }
}
