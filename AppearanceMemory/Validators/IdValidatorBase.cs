using System;
using MelonLoader.Preferences;

namespace Bnfour.MoeJigsawMods.AppearanceMemory.Validators;

internal abstract class IdValidatorBase : ValueValidator
{
    // inclusive
    protected abstract int MaxValue { get; }

    public override object EnsureValid(object value)
    {
        return 1;
    }

    public override bool IsValid(object value)
    {
        try
        {
            var unboxed = (int)value;
            return unboxed >= 1 && unboxed <= MaxValue;
        }
        catch (InvalidCastException)
        {
            return false;
        }
    }
}
