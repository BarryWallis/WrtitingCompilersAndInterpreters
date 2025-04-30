using Intermediate.Implementation;

namespace Intermediate;

/// <summary>
/// A factory for creating symbol tables.
/// </summary>
public class SymbolTableFactory
{
    /// <summary>
    /// Creates and returns a new instance of a symbol table stack.
    /// </summary>
    /// <remarks>
    /// The returned symbol table stack can be used to manage nested scopes of symbols, such as in programming language 
    /// compilers or interpreters.
    /// </remarks>
    /// <returns>
    /// An <see cref="ISymbolTableStack"/> instance that provides a stack-based structure for managing symbol tables.
    /// </returns>
    public static ISymbolTableStack CreateSymbolTableStack() => new SymbolTableStack();

    /// <summary>
    /// Create and return a new symbol table.
    /// </summary>
    /// <param name="nestingLevel">The nesting level of the symbol table.</param>
    /// <returns>An <see cref="ISymbolTable"/> instance with the specified nesting level.</returns>
    public static ISymbolTable CreateSymbolTable(int nestingLevel) => new SymbolTable(nestingLevel);

    /// <summary>
    /// Create and return a new symbol table entry.
    /// </summary>
    /// <param name="name">The name of the identifier.</param>
    /// <param name="symbolTable">The symbol table to which the entry belongs.</param>
    /// <returns>An <see cref="ISymbolTableEntry"/> instance representing the symbol table entry.</returns>
    public static ISymbolTableEntry CreateSymbolTableEntry(string name, ISymbolTable symbolTable)
        => new SymbolTableEntry(name, symbolTable);
}
