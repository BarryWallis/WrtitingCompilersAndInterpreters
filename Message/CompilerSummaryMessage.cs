namespace Messages;

/// <summary>
/// Represents a summary of a compiler's performance metrics.
/// </summary>
/// <param name="InstructionCount">
/// Indicates the total number of instructions processed during compilation.
/// </param>
/// <param name="ElapsedTime">Represents the total time taken for the compilation process.</param>
public record CompilerSummaryMessage(int InstructionCount, double ElapsedTime) : Message
{
}
