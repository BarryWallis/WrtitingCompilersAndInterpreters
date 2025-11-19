using IntermediateLib;

using MessageLib;

namespace BackendLib;

/// <summary>
/// Base class for all backend operations (code generation, interpretation, etc.).
/// Provides common message dispatch functionality and captures references to the
/// <see cref="IIntermediateCode"/> and <see cref="ISymbolTable"/> produced by the frontend.
/// </summary>
public abstract class Backend : IMessageProducer
{
    // ---------------------------------------------------------------------
    // Fields
    // ---------------------------------------------------------------------

    /// <summary>
    /// Shared message handler used by all backend instances to publish summary or diagnostic messages.
    /// Tests subscribe directly to this for verification; therefore it remains public.
    /// </summary>
    public static MessageHandler MessageHandler { get; } = new();

    /// <summary>
    /// Gets the symbol table passed to <see cref="Process"/>.
    /// </summary>
    protected ISymbolTable? SymbolTable { get; private set; } = null;

    /// <summary>
    /// Gets the intermediate code passed to <see cref="Process"/>.
    /// </summary>
    protected IIntermediateCode? IntermediateCode { get; private set; } = null;

    // ---------------------------------------------------------------------
    // Abstract API
    // ---------------------------------------------------------------------

    /// <summary>
    /// Executes the backend operation (interpretation, code generation, etc.) against the supplied
    /// intermediate representation and symbol table.
    /// </summary>
    /// <param name="intermediateCode">The intermediate code to process.</param>
    /// <param name="symbolTable">The symbol table containing declarations.</param>
    public abstract void Process(IIntermediateCode? intermediateCode, ISymbolTable? symbolTable);

    // ---------------------------------------------------------------------
    // IMessageProducer implementation
    // ---------------------------------------------------------------------

    /// <inheritdoc />
    public void AddMessageListener(IMessageListener listener) => MessageHandler.AddListener(listener);

    /// <inheritdoc />
    public void RemoveMessageListener(IMessageListener listener) => MessageHandler.RemoveListener(listener);

    /// <inheritdoc />
    public void SendMessage(Message message) => MessageHandler.SendMessage(message);
}
