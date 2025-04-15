﻿using Intermediate;

using Messages;

namespace BackendComponents;

public abstract class Backend : IMessageProducer
{
    protected static MessageHandler MessageHandler => new(); 

    protected ISymbolTable? SymbolTable { get; set; } 
    protected IIntermediateCode? IntermediateCode { get; set; } 

    /// <summary>
    /// Processes intermediate code using a symbol table for context and information.
    /// </summary>
    /// <param name="intermediateCode">
    /// Represents the intermediate representation of code to be processed.
    /// </param>
    /// <param name="symbolTable">Holds the definitions and scopes of symbols used in the code.</param>
    public abstract void Process(IIntermediateCode intermediateCode, ISymbolTable symbolTable);

    /// <summary>
    /// Adds a message listener to receive notifications for incoming messages.
    /// </summary>
    /// <param name="listener">
    /// The provided listener will handle the processing of messages as they arrive.
    /// </param>
    public abstract void AddMessageListener(IMessageListener listener);

    /// <summary>
    /// Removes a message listener from the system, preventing it from receiving further messages.
    /// </summary>
    /// <param name="listener">
    /// The listener to be removed, which previously received notifications about messages.
    /// </param>
    public abstract void RemoveMessageListener(IMessageListener listener);

    /// <summary>
    /// Sends a message to a designated recipient or system. This is an abstract method that must be 
    /// implemented by derived classes.
    /// </summary>
    /// <param name="message">The content to be transmitted to the recipient.</param>
    public abstract void SendMessage(Message message);
}
