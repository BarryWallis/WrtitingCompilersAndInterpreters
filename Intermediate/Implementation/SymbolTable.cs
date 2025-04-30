
using System.Diagnostics;

namespace Intermediate.Implementation;

/// <summary>
/// Create a new symbol table with the specified nesting level.
/// </summary>
/// <param name="nestingLevel">The nesting level of the symbol table.</param>
internal class SymbolTable(int nestingLevel) : ISymbolTable
{
    private readonly SortedDictionary<string, ISymbolTableEntry> _entries = new(StringComparer.OrdinalIgnoreCase);

    public int NestingLevel { get; } = nestingLevel;

    public IList<ISymbolTableEntry> SortedEntries => [.. _entries.Values];

    /// <inheritdoc/>
    public ISymbolTableEntry Enter(string name)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(name));

        ISymbolTableEntry entry = SymbolTableFactory.CreateSymbolTableEntry(name, this);
        _entries.Add(name, entry);
        return entry;
    }
    /// <inheritdoc/>
    public ISymbolTableEntry? Lookup(string name)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(name));

        return _entries.TryGetValue(name, out ISymbolTableEntry? entry) ? entry : null;
    }
}
