using System.Diagnostics;

using Intermediate;

using Messages;

namespace FrontendComponents;

/// <summary>
/// Abstract class for parsing, utilizing a scanner to read tokens. It maintains an error count and provides 
/// methods to parse and retrieve tokens. It will be implemented by language specific subclasses.
/// </summary>
/// <remarks>
/// Initializes a new instance of the Parser class with a specified scanner for processing input.
/// </remarks>
/// <param name="scanner">The input processing tool used to analyze and interpret data.</param>
public abstract class Parser(Scanner scanner) : IMessageProducer
{
    protected static ISymbolTable? SymbolTable { get; set; } = null;

    protected static readonly MessageHandler messageHandler = new();

    protected Scanner Scanner { get; init; } = scanner;

    protected IIntermediateCode? IntermediateCode { get; set; } = null;

    /// <summary>
    /// Represents the number of errors encountered.
    /// </summary>
    public abstract int ErrorCount { get; protected set; }

    /// <summary>
    /// Gets the current currentToken from the scanner. 
    /// </summary>
    public Token CurrentToken
    {
        get
        {
            Token? currentToken = Scanner.CurrentToken;

            Debug.Assert(currentToken is not null);
            return currentToken;
        }
    }

    /// <summary>
    /// Parses data from a source. 
    /// </summary>
    public abstract void Parse();

    /// <summary>
    /// Retrieves the next currentToken from the scanner.
    /// </summary>
    /// <returns>Returns the next currentToken available from the scanning process.</returns>
    public Token GetNextToken()
    {
        Token token = Scanner.GetNextToken();

        Debug.Assert(token is not null);
        return token;
    }

    /// <summary>
    /// Adds a listener to handle incoming messages.
    /// </summary>
    /// <param name="listener">The provided listener will be notified when a new message is received.</param>
    public void AddMessageListener(IMessageListener listener) => messageHandler.AddListener(listener);

    /// <summary>
    /// Removes a message listener from the message handler.
    /// </summary>
    /// <param name="listener">The listener to be removed from the message handling process.</param>
    public void RemoveMessageListener(IMessageListener listener) => messageHandler.RemoveListener(listener);

    /// <summary>
    /// Sends a message using the message handler.
    /// </summary>
    /// <param name="message">The object containing the details of the message to be sent.</param>
    public void SendMessage(Message message) => messageHandler.SendMessage(message);
}