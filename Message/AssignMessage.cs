namespace Messages;

/// <summary>
/// Represents a message indicating an assignment operation in the source code.
/// </summary>
/// <param name="LineNumber">The line number in the source code where the assignment occurred.</param>
/// <param name="VariableName">The name of the variable being assigned a value.</param>
/// <param name="Value">The value being assigned to the variable.</param>
/// <remarks>
/// This message is used to communicate details about assignment operations during the execution of intermediate code.
/// </remarks>
public record AssignMessage(int LineNumber, string VariableName, object? Value) : Message
{
}
