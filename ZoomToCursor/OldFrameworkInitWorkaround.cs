// i really wanted to use readonly structs with { get; init; } properties,
// so here's a workaround to enable that on Framework 3.5 with later C# versions
// see https://stackoverflow.com/questions/62648189/

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace System.Runtime.CompilerServices
#pragma warning restore IDE0130
{
    internal static class IsExternalInit { }
}
