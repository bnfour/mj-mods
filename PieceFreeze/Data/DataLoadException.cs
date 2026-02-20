using System;

namespace Bnfour.MoeJigsawMods.PieceFreeze.Data;

/// <summary>
/// Specific exception type for data loading when we know what exactly went wrong
/// and can relay the data to the user.
/// </summary>
public class DataLoadException(string message) : Exception(message) { }
