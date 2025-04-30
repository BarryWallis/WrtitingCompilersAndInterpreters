namespace Intermediate;

/// <summary>
/// The framework interface that represents the symbol table.
/// </summary>
public interface ISymbolTable
{
    int NestingLevel { get; }
    IList<ISymbolTableEntry> SortedEntries { get; }

    /// <summary>
    /// Create and enter a new entry into the symbol table.
    /// </summary>
    /// <param name="name">The name of the entry to be added.</param>
    /// <returns>The new symbol table entry.</returns>
    ISymbolTableEntry Enter(string name);

    /// <summary>
    /// Retrieves the symbol table entry associated with the specified name.
    /// </summary>
    /// <param name="name">The name of the symbol table entry to look up.</param>
    /// <returns>
    /// The <see cref="ISymbolTableEntry"/> associated with the specified name, or <see langword="null"/> if no entry is 
    /// found.
    /// </returns>
    ISymbolTableEntry? Lookup(string name);
}
