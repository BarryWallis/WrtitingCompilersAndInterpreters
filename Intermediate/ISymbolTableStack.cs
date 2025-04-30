namespace Intermediate;

/// <summary>
/// The interface for the symbol table stack.
/// </summary>
public interface ISymbolTableStack
{
    int CurrentNestingLevel { get; }
    ISymbolTable LocalSymbolTable { get; }

    /// <summary>
    /// Create and enter a new entry into the local symbol table.
    /// </summary>
    /// <param name="name">The name of the entry.</param>
    /// <returns>An <see cref="ISymbolTableEntry"/> representing the local entry.</returns>
    ISymbolTableEntry EnterLocal(string name);

    /// <summary>
    /// Look up an existing symbol table entry in the local symbol table.
    /// </summary>
    /// <param name="name">The name of the entry.</param>
    /// <returns>The entry, or <see langword="null"/> if it does not exist.
    /// t.</returns>
    ISymbolTableEntry? LookupLocal(string name);

    /// <summary>
    /// Look up an existing symbol table entry throughout the stack.
    /// </summary>
    /// <param name="name">The name of the entry.</param>
    /// <returns>The entry or <see langword="null"/> if it does not exist.</returns>
    ISymbolTableEntry? Lookup(string name);
}
