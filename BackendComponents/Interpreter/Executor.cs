using System.Diagnostics;

using Intermediate;

using Messages;

namespace BackendComponents.Interpreter;

/// <summary>
/// Executes intermediate code while tracking execution time, error count, and sending a summary message. 
/// </summary>
public class Executor : Backend
{
    private readonly MessageHandler _messageHandler = new();

    /// <summary>
    /// Processes intermediate code and symbol table, measuring execution time and counting errors.
    /// </summary>
    /// <param name="intermediateCode">Represents the code to be processed during execution.</param>
    /// <param name="symbolTable">
    /// Contains the symbols and their associated information used in the code.
    /// </param>
    public override void Process(IIntermediateCode intermediateCode, ISymbolTable symbolTable)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        stopwatch.Stop();
        double elapsedTime = stopwatch.Elapsed.TotalSeconds;
        int executionCount = 0;
        int runtimeErrors = 0;
        SendMessage(new InterpreterSummaryMessage(executionCount, runtimeErrors, elapsedTime));
    }

    /// <summary>
    /// Adds a message listener to the message handler for processing incoming messages.
    /// </summary>
    /// <param name="listener">The provided listener will handle the messages received by the system.</param>
    public override void AddMessageListener(IMessageListener listener)
        => _messageHandler.AddListener(listener);

    /// <summary>
    /// Removes a message listener from the message handler.
    /// </summary>
    /// <param name="listener">The listener to be removed from the message handling process.</param>
    public override void RemoveMessageListener(IMessageListener listener)
        => _messageHandler.RemoveListener(listener);

    /// <summary>
    /// Sends a message using the specified message handler.
    /// </summary>
    /// <param name="message">The content to be sent through the message handler.</param>
    public override void SendMessage(Message message) => _messageHandler.SendMessage(message);
}
