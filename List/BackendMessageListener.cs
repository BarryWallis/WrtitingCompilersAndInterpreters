using System.Diagnostics;

using Messages;

namespace List;

/// <summary>
/// Handles received messages and outputs execution statistics for interpreter and compiler summaries.
/// </summary>
public class BackendMessageListener : IMessageListener
{
    /// <summary>
    /// Handles different types of backend messages and outputs relevant summary information to the console.
    /// </summary>
    /// <param name="message">
    /// The message type determines information that determines the type of summary to display.
    /// </param>
    /// <exception cref="UnreachableException">
    /// Thrown when the message type does not match any expected messages.
    /// </exception>
    public void MessageReceived(Message message)
    {
        switch (message)
        {
            case InterpreterSummaryMessage interpreterSummaryMessage:
                WriteInterpreterSummaryMessage(interpreterSummaryMessage);
                break;
            case CompilerSummaryMessage compilerSummaryMessage:
                WriteCompilerSummaryMessage(compilerSummaryMessage);
                break;
            default:
                throw new UnreachableException();
        }
    }

    private static void WriteCompilerSummaryMessage(CompilerSummaryMessage compilerSummaryMessage)
    {
        Console.WriteLine();
        Console.WriteLine($"{compilerSummaryMessage.InstructionCount,20} " +
            $"instructions generated.");
        Console.WriteLine($"{compilerSummaryMessage.ElapsedTime,20:F2} " +
            $"seconds total code generation time.");
    }

    private static void WriteInterpreterSummaryMessage(InterpreterSummaryMessage interpreterSummaryMessage)
    {
        Console.WriteLine();
        Console.WriteLine($"{interpreterSummaryMessage.ExecutionCount,20} statements executed.");
        Console.WriteLine($"{interpreterSummaryMessage.RuntimeErrors,20} runtime errors.");
        Console.WriteLine($"{interpreterSummaryMessage.ElapsedTime,20:F2} " +
            $"seconds total execution time.");
        Console.WriteLine();
    }
}