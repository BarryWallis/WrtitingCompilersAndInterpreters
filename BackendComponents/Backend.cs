using Intermediate;

using Messages;

namespace BackendComponents;

public abstract class Backend : IMessageProducer
{
    public static MessageHandler MessageHandler { get; protected set; } = new();
    public static ISymbolTableStack? SymbolTableStack { get; protected set; }
    public static IIntermediateCode? IntermediateCode { get; protected set; }

    /// <summary>
    /// Processes intermediate code using a symbol table for context and information.
    /// </summary>
    /// <param name="intermediateCode">
    /// Represents the intermediate representation of code to be processed.
    /// </param>
    /// <param name="symbolTableStack">Holds the definitions and scopes of symbols used in the code.</param>
    public abstract void Process(IIntermediateCode intermediateCode, ISymbolTableStack symbolTableStack);

    /// <summary>
    /// Adds a message listener to receive notifications for incoming messages.
    /// </summary>
    /// <param name="listener">
    /// The provided listener will handle the processing of messages as they arrive.
    /// </param>
    public void AddMessageListener(IMessageListener listener) => MessageHandler.AddListener(listener);

    /// <summary>
    /// Removes a message listener from the system, preventing it from receiving further messages.
    /// </summary>
    /// <param name="listener">
    /// The listener to be removed, which previously received notifications about messages.
    /// </param>
    public void RemoveMessageListener(IMessageListener listener) => MessageHandler.RemoveListener(listener);

    /// <summary>
    /// Sends a message to a designated recipient or system. This is an abstract method that must be 
    /// implemented by derived classes.
    /// </summary>
    /// <param name="message">The content to be transmitted to the recipient.</param>
    public void SendMessage(Message message) => MessageHandler.SendMessage(message);
}
