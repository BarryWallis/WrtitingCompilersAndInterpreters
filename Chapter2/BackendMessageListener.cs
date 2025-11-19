using MessageLib;

namespace Chapter2;

/// <summary>
/// Listens for backend summary messages (compiler or interpreter) and writes metrics to the console.
/// </summary>
internal class BackendMessageListener : IMessageListener
{
    /// <summary>
    /// Receives backend summary messages and outputs execution or code generation metrics.
    /// </summary>
    /// <param name="message">The message produced by the backend.</param>
    public void MessageReceived(Message message)
    {
        switch (message)
        {
            case InterpreterSummaryMessage interpreterSummaryMessage:
                ConsoleMetricsWriter.WriteMetric(interpreterSummaryMessage.ExecutionCount, "statements executed.", leadingBlankLine: true);
                ConsoleMetricsWriter.WriteMetric(interpreterSummaryMessage.RuntimeErrors, "runtime errors.");
                ConsoleMetricsWriter.WriteSeconds(interpreterSummaryMessage.ElapsedTime.TotalSeconds, "seconds total execution time.");
                Console.WriteLine();
                break;
            case CompilerSummaryMessage compilerSummaryMessage:
                ConsoleMetricsWriter.WriteMetric(compilerSummaryMessage.InstructionCount, "instructions generated.", leadingBlankLine: true);
                ConsoleMetricsWriter.WriteSeconds(compilerSummaryMessage.ElapsedTime.TotalSeconds, "seconds total code generation time.");
                Console.WriteLine();
                break;
            default:
                message.UnknownMessageType();
                break;
        }
    }
}
