using System.Collections;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata;

using Intermediate;
using Intermediate.Implementation;

using Messages;

namespace BackendComponents.Interpreter;

/// <summary>
/// Executes assignment statements in the intermediate code by evaluating expressions and assigning values to variables.
/// </summary>
/// <remarks>
/// This class is responsible for interpreting and executing assignment statements. It evaluates the expression on the right-hand side
/// and assigns the resulting value to the variable on the left-hand side.
/// </remarks>
public class AssignmentExecutor(StatementExecutor parent) : StatementExecutor(parent)
{
    /// <summary>
    /// Executes an assignment statement represented by the given intermediate code node.
    /// </summary>
    /// <param name="node">The intermediate code node representing the assignment statement to execute.</param>
    /// <returns>
    /// Always returns <see langword="null"/> as assignment statements do not produce a direct result.
    /// </returns>
    /// <remarks>
    /// This method evaluates the expression on the right-hand side of the assignment and assigns the resulting value
    /// to the variable on the left-hand side. It also sends a message indicating the assignment operation.
    /// </remarks>
    /// <exception cref="UnreachableException">
    /// Thrown if the node structure is invalid or if required attributes are missing.
    /// </exception>
    public override object? Execute(IIntermediateCodeNode node)
    {
        // Must have a target variable and an expression.
        Debug.Assert(node.Children.Count == 2);

        IIntermediateCodeNode variableNode = node.Children[0];
        IIntermediateCodeNode expressionNode = node.Children[1];

        ExpressionExecutor expressionExecutor = new(this);
        object? value = expressionExecutor.Execute(expressionNode) /*?? throw new UnreachableException()*/;

        ISymbolTableEntry variableId = variableNode.GetAttribute(IIntermediateCodeKey.Key.Id) as ISymbolTableEntry
                                       ?? throw new UnreachableException();
        variableId.SetAttribute(SymbolTableKeyType.DataValue, value);

        SendMessage(node, variableId.Name, value);

        ++_executionCount;
        return null;
    }

    /// <summary>
    /// Sends a message indicating the assignment operation, including the variable name, value, and source line number.
    /// </summary>
    /// <param name="node">The intermediate code node representing the assignment statement.</param>
    /// <param name="variableName">The name of the variable being assigned a value.</param>
    /// <param name="value">The value being assigned to the variable.</param>
    /// <remarks>
    /// This method retrieves the line number from the node's attributes and sends a message with the assignment details.
    /// </remarks>
    private void SendMessage(IIntermediateCodeNode node, string variableName, object? value)
    {
        if (node.GetAttribute(IIntermediateCodeKey.Key.Line) is not null)
        {
            Debug.Assert(node.GetAttribute(IIntermediateCodeKey.Key.Line) is int);
            int lineNumber = (int)node.GetAttribute(IIntermediateCodeKey.Key.Line)!;
            SendMessage(new AssignMessage(lineNumber, variableName, value));
        }
    }
}
