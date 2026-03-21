using System;
using System.Collections.Generic;
using System.Text;

using IntermediateLib;

using MessageLib;

namespace BackendLib;

/// <summary>
/// Defines the base contract for backend components that process intermediate code
/// and publish messages to registered listeners.
/// </summary>
public abstract class Backend : IMessageProducer
{
    protected static MessageHandler MessageHandler { get; } = new();

    /// <summary>
    /// Processes intermediate code using the provided symbol table.
    /// </summary>
    /// <param name="intermediateCode">The intermediate code to process.</param>
    /// <param name="symbolTable">The symbol table associated with the intermediate code.</param>
    public abstract void Process(IIntermediateCode intermediateCode, ISymbolTable symbolTable);

    /// <summary>
    /// Adds a message listener to receive backend messages.
    /// </summary>
    /// <param name="listener">The listener to register.</param>
    public void AddMessageListener(IMessageListener listener) => MessageHandler.AddListener(listener);

    /// <summary>
    /// Removes a previously registered message listener.
    /// </summary>
    /// <param name="listener">The listener to remove.</param>
    public void RemoveMessageListener(IMessageListener listener) => MessageHandler.RemoveListener(listener);

    /// <summary>
    /// Sends a message to all registered listeners.
    /// </summary>
    /// <param name="message">The message to send.</param>
    public void SendMessage(Message message) => MessageHandler.SendMessage(message);
}
