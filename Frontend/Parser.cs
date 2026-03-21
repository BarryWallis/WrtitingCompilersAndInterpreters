using IntermediateLib;

using MessageLib;

namespace FrontendLib;

/// <summary>
/// A language-independent framework class. This abstract parser class will be implemented by language-specific subclasses.
/// </summary>
public abstract class Parser(Scanner theScanner) : IMessageProducer
{
    protected Scanner scanner = theScanner;
    protected IIntermediateCode? intermediateCode = null;

    protected static ISymbolTable? SymbolTable { get; set; }
    protected static MessageHandler MessageHandler { get; } = new();

    /// <summary>
    /// Gets the number of syntax errors found by the parser.
    /// To be implemented by a language-specific parser subclass.
    /// </summary>
    /// <value>The syntax error count.</value>
    public abstract int ErrorCount { get; }

    /// <summary>
    /// Parse a source program and generate the intermediate code and the symbol table.
    /// To be implemented by a language-specific parser subclass.
    /// </summary>
    public abstract void Parse();

    /// <summary>
    /// Call the scanner's CurrentToken() method.
    /// </summary>
    /// <returns>The current token.</returns>
    public Token CurrentToken() => scanner.CurrentToken();

    /// <summary>
    /// Call the scanner's NextToken() method.
    /// </summary>
    /// <returns>The next token.</returns>
    /// <exception cref="Exception">If an error occurred.</exception>
    public Token NextToken() => scanner.NextToken();

    /// <summary>
    /// Add a parser message listener.
    /// </summary>
    /// <param name="listener">The message listener to add.</param>
    public void AddMessageListener(IMessageListener listener) => MessageHandler.AddListener(listener);

    /// <summary>
    /// Remove a parser message listener.
    /// </summary>
    /// <param name="listener">The message listener to remove.</param>
    public void RemoveMessageListener(IMessageListener listener) => MessageHandler.RemoveListener(listener);

    /// <summary>
    /// Notify listeners after setting the message.
    /// </summary>
    /// <param name="message">The message to set.</param>
    public void SendMessage(Message message) => MessageHandler.SendMessage(message);
}
