using Intermediate.Implementation;

namespace Intermediate;

/// <summary>
/// The interface for a symbol table entry.
/// </summary>
public interface ISymbolTableEntry
{
    string Name { get; }
    ISymbolTable SymbolTable { get; }
    IList<int> LineNumbers { get; }

    /// <summary>
    /// Append a source line number to the entry.
    /// </summary>
    /// <param name="lineNumber">The line number to append.</param>
    void AppendLineNumber(int lineNumber);

    /// <summary>
    /// Sets the value of the specified attribute for this symbol table entry.
    /// </summary>
    /// <remarks>This method updates the value associated with the given key in the symbol table.  If the key
    /// does not already exist, it will be added with the specified value.</remarks>
    /// <param name="key">The key representing the attribute to set.</param>
    /// <param name="value">The value to associate with the specified key.</param>
    void SetAttribute(ISymbolTableKey<SymbolTableKeyType> key, object? value);

    /// <summary>
    /// Retrieves the value associated with the specified key from the symbol table entry.
    /// </summary>
    /// <param name="key">The key used to identify the attribute in the symbol table.</param>
    /// <returns>The value associated with the specified key.
    /// </returns>
    object? GetAttribute(ISymbolTableKey<SymbolTableKeyType> key);
}
