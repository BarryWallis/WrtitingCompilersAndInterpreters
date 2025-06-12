using System.Diagnostics;

using Messages;

namespace SyntaxChecker;

/// <summary>
/// Handles received messages and outputs execution statistics for interpreter and compiler summaries.
/// </summary>
public class BackendMessageListener : IMessageListener
{
    private bool _firstOutputMessage = true;

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
            case AssignMessage assignMessage:
                if (_firstOutputMessage)
                {
                    Console.WriteLine();
                    Console.WriteLine("===== OUTPUT =====");
                    Console.WriteLine();
                    _firstOutputMessage = false;
                }

                Console.WriteLine($" >>> Line {assignMessage.LineNumber:000}: " +
                    $"{assignMessage.VariableName} = {assignMessage.Value}");
                break;
            case RuntimeErrorMessage runtimeErrorMessage:
                Console.Write("*** RUNTIME ERROR");
                if (runtimeErrorMessage.LineNumber is not null)
                {
                    Console.Write($" AT LINE {runtimeErrorMessage.LineNumber.Value:000}");
                }
                Console.WriteLine($": {runtimeErrorMessage.ErrorMessage}");
                break;
                // TODO: Handle other message types as needed
                //default:
                //    throw new UnreachableException();
        }
    }

    /// <summary>
    /// Outputs a summary of the compiler's execution statistics to the console.
    /// </summary>
    /// <param name="compilerSummaryMessage">The message containing compiler summary details, such as instruction count and elapsed time.</param>
    private static void WriteCompilerSummaryMessage(CompilerSummaryMessage compilerSummaryMessage)
    {
        Console.WriteLine();
        Console.WriteLine($"{compilerSummaryMessage.InstructionCount,20} " +
            $"instructions generated.");
        Console.WriteLine($"{compilerSummaryMessage.ElapsedTime,20:F2} " +
            $"seconds total code generation time.");
    }

    /// <summary>
    /// Outputs a summary of the interpreter's execution statistics to the console.
    /// </summary>
    /// <param name="interpreterSummaryMessage">The message containing interpreter summary details, such as execution count, runtime errors, and elapsed time.</param>
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
