using System.Diagnostics;

using Intermediate;

using Messages;

namespace BackendComponents.Interpreter;

/// <summary>
/// Executes statements in the intermediate code by delegating to specific executors for different statement types.
/// </summary>
/// <remarks>
/// This class is responsible for interpreting and executing statements in the intermediate code representation.
/// It delegates execution to specialized executors for compound statements, assignments, and other types of statements.
/// </remarks>
public class StatementExecutor(Executor parent) : Executor(parent)
{
    /// <summary>
    /// Executes a statement represented by the given intermediate code node.
    /// </summary>
    /// <param name="node">The intermediate code node representing the statement to execute.</param>
    /// <returns>
    /// The result of executing the statement, or <see langword="null"/> if the statement does not produce a result.
    /// </returns>
    /// <remarks>
    /// This method determines the type of the statement and delegates execution to the appropriate executor.
    /// </remarks>
    public virtual object? Execute(IIntermediateCodeNode node)
    {
        IIntermediateCodeNodeType.Kind nodeType = node.Kind;

        // Send a message indicating the source line of the statement being executed.
        SendSourceLineMessage(node);

        switch (nodeType)
        {
            case IIntermediateCodeNodeType.Kind.Compound:
                // Delegate execution to the CompoundExecutor for compound statements.
                CompoundExecutor compoundExecutor = new(this);
                return compoundExecutor.Execute(node);
            case IIntermediateCodeNodeType.Kind.Assign:
                // Delegate execution to the AssignmentExecutor for assignment statements.
                AssignmentExecutor assignmentExecutor = new(this);
                return assignmentExecutor.Execute(node);
            case IIntermediateCodeNodeType.Kind.No_Op:
                // No operation; return null.
                return null;
            default:
                // Flag an error for unimplemented statement types.
                RuntimeErrorHandler.Flag(node, RuntimeErrorCode.UnimplementedFeature, this);
                return null;
        }
    }

    /// <summary>
    /// Sends a message indicating the source line of the statement being executed.
    /// </summary>
    /// <param name="node">The intermediate code node representing the statement.</param>
    /// <remarks>
    /// This method retrieves the line number from the node's attributes and sends a message with the line number.
    /// </remarks>
    private void SendSourceLineMessage(IIntermediateCodeNode node)
    {
        Debug.Assert(node.GetAttribute(IIntermediateCodeKey.Key.Line) is not null and int);
        int? lineNumber = (int)node.GetAttribute(IIntermediateCodeKey.Key.Line)!;
        if (lineNumber is not null)
        {
            SendMessage(new SourceLineMessage(lineNumber.Value));
        }
    }
}
