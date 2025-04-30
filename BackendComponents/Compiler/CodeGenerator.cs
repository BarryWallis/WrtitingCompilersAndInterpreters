using System.Diagnostics;

using Intermediate;

using Messages;

namespace BackendComponents.Compiler;

/// <summary>
/// Generates code by processing intermediate code and symbol tables. Measures processing time and sends a 
/// summary message.
/// </summary>
public class CodeGenerator : Backend
{
    private static readonly MessageHandler _messageHandler = new();

    /// <summary>
    /// Processes intermediate code and symbol table, measuring execution time and counting instructions.
    /// </summary>
    /// <param name="intermediateCode">
    /// Represents the intermediate representation of the code to be processed.
    /// </param>
    /// <param name="symbolTable"
    /// >Holds the symbols and their associated information for the code being processed.
    /// </param>
    public override void Process(IIntermediateCode intermediateCode, ISymbolTableStack symbolTableStack)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        stopwatch.Stop();
        double elapsedTime = stopwatch.Elapsed.TotalSeconds;
        int instructionCount = 0;
        SendMessage(new CompilerSummaryMessage(instructionCount, elapsedTime));
    }

    /// <summary>
    /// Adds a message listener to the message handler for processing incoming messages.
    /// </summary>
    /// <param name="listener">The listener is used to handle messages when they are received.</param>
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
