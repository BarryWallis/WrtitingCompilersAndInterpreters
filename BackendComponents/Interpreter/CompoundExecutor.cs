using Intermediate;

namespace BackendComponents.Interpreter;

/// <summary>
/// Executes compound statements in the intermediate code by iterating through their child nodes.
/// </summary>
/// <remarks>
/// This class is responsible for interpreting and executing compound statements, which consist of multiple child statements.
/// It delegates the execution of each child statement to a <see cref="StatementExecutor"/>.
/// </remarks>
public class CompoundExecutor(StatementExecutor parent) : StatementExecutor(parent)
{
    /// <summary>
    /// Executes a compound statement represented by the given intermediate code node.
    /// </summary>
    /// <param name="node">The intermediate code node representing the compound statement to execute.</param>
    /// <returns>
    /// Always returns <see langword="null"/> as compound statements do not produce a direct result.
    /// </returns>
    /// <remarks>
    /// This method iterates through the child nodes of the compound statement and executes each one using a <see cref="StatementExecutor"/>.
    /// </remarks>
    public override object? Execute(IIntermediateCodeNode node)
    {
        StatementExecutor statementExecutor = new(this);
        foreach (IIntermediateCodeNode child in node.Children)
        {
            _ = statementExecutor.Execute(child);
        }

        return null;
    }
}
