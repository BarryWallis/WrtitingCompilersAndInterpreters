namespace MessageLib;

/// <summary>
/// Summary message produced after interpretation (execution) completes.
/// </summary>
/// <param name="ExecutionCount">Total number of statements executed.</param>
/// <param name="RuntimeErrors">Total number of runtime errors encountered.</param>
/// <param name="ElapsedTime">Total elapsed execution time.</param>
public record InterpreterSummaryMessage(int ExecutionCount, int RuntimeErrors, TimeSpan ElapsedTime) : Message;
