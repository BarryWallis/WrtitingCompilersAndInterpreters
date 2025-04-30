namespace Intermediate.Implementation;

/// <inheritdoc/>
public class SymbolTableStack : ISymbolTableStack
{
    private readonly IList<ISymbolTable> _symbolTables = [];

    public ISymbolTable LocalSymbolTable => _symbolTables[CurrentNestingLevel];
    public int CurrentNestingLevel { get; private set; } = 0;

    /// <summary>
    /// Create a new symbol table stack with the initial symbol table.
    /// </summary>
    public SymbolTableStack() => _symbolTables.Add(SymbolTableFactory.CreateSymbolTable(CurrentNestingLevel));

    /// <inheritdoc/>
    public ISymbolTableEntry EnterLocal(string name) => _symbolTables[CurrentNestingLevel].Enter(name);

    /// <inheritdoc/>
    public ISymbolTableEntry? Lookup(string name) => LookupLocal(name);

    /// <inheritdoc/>
    public ISymbolTableEntry? LookupLocal(string name) => _symbolTables[CurrentNestingLevel].Lookup(name);
}
