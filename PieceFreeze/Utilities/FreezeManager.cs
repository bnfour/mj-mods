using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HarmonyLib;
using MelonLoader;

using Jigsaw.Piece;

using Bnfour.MoeJigsawMods.PieceFreeze.Data;

namespace Bnfour.MoeJigsawMods.PieceFreeze.Utilities;

// this class is only static to simplify calls to it in transpiler
// (i'd rather prefer to emit less CIL by hand)

internal static class FreezeManager
{
    private static readonly int[] LinkIndices = [.. Enum.GetValues(typeof(Model.LINKPOS)).Cast<int>()];

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
            if (mod.LockedData.Current.Contains(model.gameObject.name))
            {
                Unlock(mod, model);
            }
            else
            {
                Lock(mod, model);
            }
            // still disallow usual interaction
            return true;
        }

        var locked = mod.LockedData.Current.Contains(model.gameObject.name);
        if (locked)
        {
            Highlight(model, Palette.Locked);
            if (mod.EnableSound)
            {
                SoundTable2.Instance.PlaySE(SoundTable2.SE.Select);
            }
        }

        return locked;
    }

    /// <summary>
    /// Locks the piece if it's now connected to a locked section.
    /// Called on docking.
    /// </summary>
    /// <param name="model">Piece to process.</param>
    internal static void ProcessDocking(Model model)
    {
        var mod = Melon<PieceFreezeMod>.Instance;
        if (IsConnectedToAnyLockedNow(mod.LockedData.Current, model))
        {
            Lock(mod, model, true);
        }
    }

    private static void Lock(PieceFreezeMod modInstance, Model model, bool isDocking = false)
    {
        var pieceName = model.gameObject.name;

        Debug.Assert(!modInstance.LockedData.Current.Contains(pieceName), "locking a locked piece");
        modInstance.LockedData.Current.Add(pieceName);

        Highlight(model, Palette.Locking);
        if (!isDocking && modInstance.EnableSound)
        {
            SoundTable2.Instance.PlaySE(SoundTable2.SE.Focus);
        }

        foreach (int i in LinkIndices)
        {
            if (model.Link[i] != null && !modInstance.LockedData.Current.Contains(model.Link[i].gameObject.name))
            {
                Lock(modInstance, model.Link[i]);
            }
        }
    }

    private static void Unlock(PieceFreezeMod modInstance, Model model)
    {
        var pieceName = model.gameObject.name;

        Debug.Assert(modInstance.LockedData.Current.Contains(pieceName), "unlocking non-locked piece");
        modInstance.LockedData.Current.Remove(pieceName);

        Highlight(model, Palette.Unlocking);
        if (modInstance.EnableSound)
        {
            SoundTable2.Instance.PlaySE(SoundTable2.SE.Focus);
        }

        foreach (int i in LinkIndices)
        {
            if (model.Link[i] != null && modInstance.LockedData.Current.Contains(model.Link[i].gameObject.name))
            {
                Unlock(modInstance, model.Link[i]);
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

        foreach (int i in LinkIndices)
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

    // (ab)uses the docking highlight effect with a custom color
    private static void Highlight(Model model, UnityEngine.Color color)
    {
        var dockingMaterial = Traverse.Create(model).Field("matDocking");
        dockingMaterial.Method("SetColor", "_EmissionColor", color).GetValue();

        model.Docking();

        dockingMaterial.Method("SetColor", "_EmissionColor", UnityEngine.Color.white).GetValue();
    }
}
