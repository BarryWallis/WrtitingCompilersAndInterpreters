namespace MessageLib;

/// <summary>
/// Summary message produced after code generation completes.
/// </summary>
/// <param name="InstructionCount">Total number of target instructions generated.</param>
/// <param name="ElapsedTime">Total elapsed code generation time.</param>
public record CompilerSummaryMessage(int InstructionCount, TimeSpan ElapsedTime) : Message;
