namespace Messages;

public record InterpreterSummaryMessage(int ExecutionCount, int RuntimeErrors, double ElapsedTime) : Message
{
}
