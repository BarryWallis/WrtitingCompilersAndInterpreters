using System.Diagnostics;

using IntermediateLib;

using MessageLib;

namespace FrontendLib;

/// <summary>
/// Base parser that coordinates scanning tokens and producing messages during parsing.
/// </summary>
/// <remarks>
/// Concrete parsers should implement <see cref="Parse"/> and provide their own error counting.
/// </remarks>
public abstract class Parser(Scanner scanner) : IMessageProducer
{
    // ---------------------------------------------------------------------
    // Fields
    // ---------------------------------------------------------------------

    protected static readonly MessageHandler _messageHandler = new();
    protected readonly Scanner _scanner = scanner ?? throw new ArgumentNullException(nameof(scanner));

    // ---------------------------------------------------------------------
    // Properties
    // ---------------------------------------------------------------------

    /// <summary>
    /// Gets the current number of parsing errors.
    /// </summary>
    public abstract int ErrorCount { get; }

    /// <summary>
    /// Gets the intermediate code structure built during parsing (if implemented by the concrete parser).
    /// </summary>
    public IIntermediateCode? IntermediateCode { get; private set; }

    /// <summary>
    /// Gets the symbol table populated during parsing (if implemented by the concrete parser).
    /// </summary>
    public ISymbolTable? SymbolTable { get; private set; }

    /// <summary>
    /// Gets the current token from the underlying <see cref="Scanner"/>.
    /// </summary>
    public Token CurrentToken => _scanner.CurrentToken ?? throw new UnreachableException();

    // ---------------------------------------------------------------------
    // Public methods
    // ---------------------------------------------------------------------

    /// <summary>
    /// Retrieves the next token from the underlying scanner.
    /// </summary>
    public Token GetNextToken() => _scanner.GetNextToken();

    /// <summary>
    /// Starts the parsing process. Implementations should consume tokens and produce messages.
    /// </summary>
    public abstract void Parse();

    /// <summary>
    /// Adds a listener to receive messages emitted by the parser.
    /// </summary>
    public void AddMessageListener(IMessageListener listener) => _messageHandler.AddListener(listener);

    /// <summary>
    /// Removes a listener from receiving messages emitted by the parser.
    /// </summary>
    public void RemoveMessageListener(IMessageListener listener) => _messageHandler.RemoveListener(listener);

    /// <summary>
    /// Sends a message to all registered listeners.
    /// </summary>
    public void SendMessage(Message message) => _messageHandler.SendMessage(message);
}
