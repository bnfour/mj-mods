using System;
using System.Collections.Generic;
using System.Diagnostics;
using MelonLoader;

using Jigsaw.Piece;

namespace Bnfour.MoeJigsawMods.PieceFreeze.Utilities;

// this class is only static to simplify calls to it in transpiler
// (i'd rather prefer to emit less CIL by hand)

internal static class FreezeManager
{
    /// <summary>
    /// Checks if the piece user tries to interact with is frozen.
    /// </summary>
    /// <param name="model">Model of the piece user tried to grab or rotate.</param>
    /// <param name="isLeftMouseButton">Whether the left mouse button was used for grabbing
    /// (as opposed to right mouse button or A/D/arrows for rotating).
    /// Used to toggle the state on left-clicking with the modifier held.</param>
    /// <returns>Whether the piece is frozen, and the interaction attempt should be cancelled.</returns>
    internal static bool IsFrozen(Model model, bool isLeftMouseButton)
    {
        // only handle pieces lying on the field
        if (model?.Place != Model.PLACE.MAIN)
        {
            return false;
        }

        var mod = Melon<PieceFreezeMod>.Instance;

        // if clicked with a modifier, toggle the lock state
        if (isLeftMouseButton && mod.FreezeModifierDown)
        {
            if (mod.LockedPieces.Contains(model.gameObject.name))
            {
                Unlock(mod.LockedPieces, model);
            }
            else
            {
                Lock(mod.LockedPieces, model);
            }
            // still disallow usual interaction
            return true;
        }

        return mod.LockedPieces.Contains(model.gameObject.name);
    }

    /// <summary>
    /// Locks the piece if it's now connected to a locked section.
    /// Called on docking.
    /// </summary>
    /// <param name="model">Piece to process.</param>
    internal static void ProcessDocking(Model model)
    {
        var lockedPieces = Melon<PieceFreezeMod>.Instance.LockedPieces;
        if (IsConnectedToAnyLockedNow(lockedPieces, model))
        {
            Lock(lockedPieces, model);
        }
    }

    private static void Lock(HashSet<string> lockedPieces, Model model)
    {
        var pieceName = model.gameObject.name;

        Debug.Assert(!lockedPieces.Contains(pieceName), "locking a locked piece");
        lockedPieces.Add(pieceName);

        foreach (int i in Enum.GetValues(typeof(Model.PLACE)))
        {
            if (model.Link[i] != null && !lockedPieces.Contains(model.Link[i].gameObject.name))
            {
                Lock(lockedPieces, model.Link[i]);
            }
        }
    }

    private static void Unlock(HashSet<string> lockedPieces, Model model)
    {
        var pieceName = model.gameObject.name;

        Debug.Assert(lockedPieces.Contains(pieceName), "unlocking non-locked piece");
        lockedPieces.Remove(pieceName);

        foreach (int i in Enum.GetValues(typeof(Model.PLACE)))
        {
            if (model.Link[i] != null && lockedPieces.Contains(model.Link[i].gameObject.name))
            {
                Unlock(lockedPieces, model.Link[i]);
            }
        }
    }

    private static bool IsConnectedToAnyLockedNow(HashSet<string> lockedPieces, Model model, HashSet<string> alreadyChecked = null)
    {
        alreadyChecked ??= new();

        var pieceName = model.gameObject.name;

        if (lockedPieces.Contains(pieceName))
        {
            return true;
        }

        alreadyChecked.Add(pieceName);

        foreach (int i in Enum.GetValues(typeof(Model.PLACE)))
        {
            if (model.Link[i] != null && !alreadyChecked.Contains(model.Link[i].gameObject.name))
            {
                if (IsConnectedToAnyLockedNow(lockedPieces, model.Link[i], alreadyChecked))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
