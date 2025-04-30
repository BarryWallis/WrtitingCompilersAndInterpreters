
using System.Diagnostics;

namespace Intermediate.Implementation;

/// <summary>
/// A symbol table entry that represents a single entry in the symbol table.
/// </summary>
public class SymbolTableEntry : ISymbolTableEntry
{
    private readonly Dictionary<ISymbolTableKey<SymbolTableKeyType>, object?> _attributes = [];

    public string Name { get; }
    public ISymbolTable SymbolTable { get; }
    public IList<int> LineNumbers { get; } = [];

    /// <summary>
    /// Create a new symbol table entry with the specified name and symbol table.
    /// </summary>
    /// <param name="name">The name of the symbol table entry. Cannot be null or whitespace.</param>
    /// <param name="symbolTable">The symbol table associated with this entry.</param>
    public SymbolTableEntry(string name, ISymbolTable symbolTable)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(name), "Name cannot be null or whitespace.");

        Name = name;
        SymbolTable = symbolTable;
    }
    /// <summary>
    /// Appends a line number to the collection of line numbers.
    /// </summary>
    /// <remarks>This method adds the specified line number to the <see cref="LineNumbers"/> collection. 
    /// Duplicate line numbers are allowed.</remarks>
    /// <param name="lineNumber">The line number to append. Must be a positive integer.</param>
    public void AppendLineNumber(int lineNumber)
    {
        Debug.Assert(lineNumber > 0, "Line number must be a positive integer.");

        LineNumbers.Add(lineNumber);
    }

    /// <summary>
    /// Returns the value of the specified attribute for this symbol table entry.
    /// </summary>
    /// <param name="key">The key of the attribute to retrieve.</param>
    /// <returns>The value of the attribute, or null if the attribute does not exist.</returns>
    public object? GetAttribute(ISymbolTableKey<SymbolTableKeyType> key)
        => _attributes.TryGetValue(key, out object? value) ? value : null;

    /// <summary>
    /// Adds or updates the value of the specified attribute for this symbol table entry.
    /// </summary>
    /// <param name="key">The key of the attribute to set.</param>
    /// <param name="value">The value of the attribute. Can be null.</param>
    public void SetAttribute(ISymbolTableKey<SymbolTableKeyType> key, object? value) => _attributes[key] = value;
}
