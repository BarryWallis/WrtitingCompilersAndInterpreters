using System.Collections.Immutable;
using System.Diagnostics;

using Intermediate;
using Intermediate.Implementation;

using Messages;

namespace BackendComponents.Interpreter;

/// <summary>
/// Executes expressions in the intermediate code by evaluating their values and performing operations.
/// </summary>
/// <remarks>
/// This class is responsible for interpreting and evaluating expressions, including variables, constants,
/// arithmetic operations, and logical operations.
/// </remarks>
public class ExpressionExecutor(StatementExecutor parent) : StatementExecutor(parent)
{
    /// <summary>
    /// A set of arithmetic operators supported by the executor.
    /// </summary>
    private static readonly HashSet<IIntermediateCodeNodeType.Kind> _arithmeticOperators =
        [
            IIntermediateCodeNodeType.Kind.Add,
            IIntermediateCodeNodeType.Kind.Subtract,
            IIntermediateCodeNodeType.Kind.Multiply,
            IIntermediateCodeNodeType.Kind.Float_Divide,
            IIntermediateCodeNodeType.Kind.Integer_Divide
        ];

    /// <summary>
    /// Executes an expression represented by the given intermediate code node.
    /// </summary>
    /// <param name="node">The intermediate code node representing the expression to execute.</param>
    /// <returns>
    /// The result of evaluating the expression, which can be an integer, float, string, or boolean value.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// Thrown if the node structure is invalid or if required attributes are missing.
    /// </exception>
    public override object? Execute(IIntermediateCodeNode node)
    {
        IIntermediateCodeNodeType.Kind nodeType = node.Kind;
        switch (nodeType)
        {
            case IIntermediateCodeNodeType.Kind.Variable:
                // Handle variable expressions
                ISymbolTableEntry? entry = node.GetAttribute(IIntermediateCodeKey.Key.Id) as ISymbolTableEntry;
                //?? throw new UnreachableException("Variable node must have a symbol table entry.");
                return entry?.GetAttribute(SymbolTableKeyType.DataValue);
            //?? throw new UnreachableException("Variable must have a value.");
            case IIntermediateCodeNodeType.Kind.Integer_Constant:
                // Handle integer constant expressions
                Debug.Assert(node.GetAttribute(IIntermediateCodeKey.Key.Value) is int, "Integer constant must have a value.");
                return (int)node.GetAttribute(IIntermediateCodeKey.Key.Value)!;
            case IIntermediateCodeNodeType.Kind.Real_Constant:
                // Handle real constant expressions
                Debug.Assert(node.GetAttribute(IIntermediateCodeKey.Key.Value) is float, "Real constant must have a value.");
                return (float)node.GetAttribute(IIntermediateCodeKey.Key.Value)!;
            case IIntermediateCodeNodeType.Kind.String_Constant:
                // Handle string constant expressions
                return node.GetAttribute(IIntermediateCodeKey.Key.Value) as string
                       ?? throw new UnreachableException("String constant must have a value.");
            case IIntermediateCodeNodeType.Kind.Negate:
                return HandleNegate(node);
            case IIntermediateCodeNodeType.Kind.Not:
                return HandleNot(node);
            default:
                return ExecuteBinaryOperator(node, nodeType);
        }
    }

    /// <summary>
    /// Handles the negation of an expression.
    /// </summary>
    /// <param name="node">The intermediate code node representing the negation operation.</param>
    /// <returns>
    /// The negated value of the expression, which can be an integer or float.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// Thrown if the node structure is invalid or if the value is not numeric.
    /// </exception>
    private object HandleNegate(IIntermediateCodeNode node)
    {
        IList<IIntermediateCodeNode> children = node.Children;
        IIntermediateCodeNode expressionNode =
            children.Count == 1 ? children[0] : throw new UnreachableException("Negation must have an expression child.");
        object? value = Execute(expressionNode);
        if (value is int intValue)
        {
            return -intValue; // Negate integer
        }
        else if (value is float floatValue)
        {
            return -floatValue; // Negate float
        }
        else
        {
            throw new UnreachableException("Negation can only be applied to numeric types (int or float).");
        }
    }

    /// <summary>
    /// Handles the logical NOT operation on a boolean expression.
    /// </summary>
    /// <param name="node">The intermediate code node representing the NOT operation.</param>
    /// <returns>
    /// The negated boolean value of the expression.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// Thrown if the node structure is invalid or if the value is not boolean.
    /// </exception>
    private object HandleNot(IIntermediateCodeNode node)
    {
        IList<IIntermediateCodeNode> notChildren = node.Children;
        IIntermediateCodeNode notExpressionNode =
            notChildren.Count == 1 ? notChildren[0]
                                   : throw new UnreachableException("Not operation must have an expression child.");

        object? boolValue = Execute(notExpressionNode);
        if (boolValue is bool booleanValue)
        {
            return !booleanValue; // Negate boolean
        }
        else
        {
            throw new UnreachableException("Not operation can only be applied to boolean types.");
        }
    }

    /// <summary>
    /// Executes a binary operator represented by the given intermediate code node.
    /// </summary>
    /// <param name="node">The intermediate code node representing the binary operator.</param>
    /// <param name="nodeType">The type of the binary operator.</param>
    /// <returns>
    /// The result of evaluating the binary operator, which can be an integer, float, or boolean value.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// Thrown if the node structure is invalid or if required attributes are missing.
    /// </exception>
    private object? ExecuteBinaryOperator(IIntermediateCodeNode node, IIntermediateCodeNodeType.Kind nodeType)
    {
        IList<IIntermediateCodeNode> children = node.Children;
        Debug.Assert(children.Count == 2, "Binary operators must have exactly two operands.");
        IIntermediateCodeNode operandNode1 = children[0];
        IIntermediateCodeNode operandNode2 = children[1];

        object? operand1 = Execute(operandNode1);
        object? operand2 = Execute(operandNode2);

        bool integerMode = operand1 is int && operand2 is int;

        return _arithmeticOperators.Contains(nodeType)
               ? integerMode ? HandleIntegerMode(node, nodeType, operand1, operand2)
                             : HandleFloatMode(node, nodeType, operand1, operand2)
               : nodeType is IIntermediateCodeNodeType.Kind.And or IIntermediateCodeNodeType.Kind.Or
                 ? HandleLogicalMode(nodeType, operand1, operand2)
                 : integerMode ? HandleIntegerRelations(nodeType, operand1, operand2)
                                 : HandleFloatRelations(nodeType, operand1, operand2);
    }

    /// <summary>
    /// Handles relational operations for floating-point operands.
    /// </summary>
    /// <param name="nodeType">The type of the relational operator.</param>
    /// <param name="operand1">The first operand of the relation.</param>
    /// <param name="operand2">The second operand of the relation.</param>
    /// <returns>
    /// A boolean value indicating the result of the relational operation.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// Thrown if the operands are not numeric.
    /// </exception>
    private static bool HandleFloatRelations(IIntermediateCodeNodeType.Kind nodeType, object? operand1, object? operand2)
    {
        float value1 = operand1 is int intValue1 ? intValue1
                                                : operand1 is float floatValue1 ? floatValue1
                                                : throw new UnreachableException($"{nameof(operand1)} is not arithmetic");
        float value2 = operand2 is int intValue2 ? intValue2
                                                 : operand2 is float floatValue2 ? floatValue2
                                                 : throw new UnreachableException($"{nameof(operand2)} is not arithmetic");

        return nodeType switch
        {
            IIntermediateCodeNodeType.Kind.Eq => value1 == value2,
            IIntermediateCodeNodeType.Kind.Ne => value1 != value2,
            IIntermediateCodeNodeType.Kind.Lt => value1 < value2,
            IIntermediateCodeNodeType.Kind.Le => value1 <= value2,
            IIntermediateCodeNodeType.Kind.Gt => value1 > value2,
            IIntermediateCodeNodeType.Kind.Ge => value1 >= value2,
            _ => throw new UnreachableException("Unknown relational operator.")
        };
    }

    /// <summary>
    /// Handles relational operations for integer operands.
    /// </summary>
    /// <param name="nodeType">The type of the relational operator.</param>
    /// <param name="operand1">The first operand of the relation.</param>
    /// <param name="operand2">The second operand of the relation.</param>
    /// <returns>
    /// A boolean value indicating the result of the relational operation.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// Thrown if the operands are not integers.
    /// </exception>
    private static bool HandleIntegerRelations(IIntermediateCodeNodeType.Kind nodeType, object? operand1, object? operand2)
    {
        int value1 = operand1 is not null ? (int)operand1 : throw new UnreachableException();
        int value2 = operand2 is not null ? (int)operand2 : throw new UnreachableException();

        return nodeType switch
        {
            IIntermediateCodeNodeType.Kind.Eq => value1 == value2,
            IIntermediateCodeNodeType.Kind.Ne => value1 != value2,
            IIntermediateCodeNodeType.Kind.Lt => value1 < value2,
            IIntermediateCodeNodeType.Kind.Le => value1 <= value2,
            IIntermediateCodeNodeType.Kind.Gt => value1 > value2,
            IIntermediateCodeNodeType.Kind.Ge => value1 >= value2,
            _ => throw new UnreachableException("Unknown relational operator.")
        };
    }

    /// <summary>
    /// Handles logical operations (AND, OR) for boolean operands.
    /// </summary>
    /// <param name="nodeType">The type of the logical operator.</param>
    /// <param name="operand1">The first operand of the logical operation.</param>
    /// <param name="operand2">The second operand of the logical operation.</param>
    /// <returns>
    /// A boolean value indicating the result of the logical operation.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// Thrown if the operands are not boolean.
    /// </exception>
    private static bool? HandleLogicalMode(IIntermediateCodeNodeType.Kind nodeType, object? operand1, object? operand2)
    {
        bool? value1 = operand1 switch
        {
            bool value => value,
            _ => null /*throw new UnreachableException($"{nameof(operand1)} is not arithmetic")*/
        };

        bool? value2 = operand2 switch
        {
            bool value => value,
            _ => null /*throw new UnreachableException($"{nameof(operand2)} is not arithmetic")*/
        };

        // Handle AND / OR operators  
        return nodeType switch
        {
            IIntermediateCodeNodeType.Kind.And => value1 is null || value2 is null ? null : value1.Value && value2.Value,
            IIntermediateCodeNodeType.Kind.Or => value1 is null || value2 is null ? null : value1.Value || value2.Value,
            _ => throw new UnreachableException("Unknown relational operator.")
        };
    }

    /// <summary>
    /// Handles arithmetic operations for floating-point operands.
    /// </summary>
    /// <param name="node">The intermediate code node representing the arithmetic operation.</param>
    /// <param name="nodeType">The type of the arithmetic operator.</param>
    /// <param name="operand1">The first operand of the arithmetic operation.</param>
    /// <param name="operand2">The second operand of the arithmetic operation.</param>
    /// <returns>
    /// The result of the arithmetic operation as a float.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// Thrown if the operands are not numeric.
    /// </exception>
    private float? HandleFloatMode(IIntermediateCodeNode node, IIntermediateCodeNodeType.Kind nodeType, object? operand1,
                                   object? operand2)
    {
        float? value1 = operand1 is int intValue1 ? intValue1
                                                 : operand1 is float floatValue1 ? floatValue1
                                                 : null /*throw new UnreachableException($"{nameof(operand1)} is not arithmetic")*/;
        float? value2 = operand2 is int intValue2 ? intValue2
                                                 : operand2 is float floatValue2 ? floatValue2
                                                 : null /*throw new UnreachableException($"{nameof(operand2)} is not arithmetic")*/;
        if (value1 is null || value2 is null)
        {
            return null;
        }

        if (nodeType == IIntermediateCodeNodeType.Kind.Float_Divide && value2 == 0.0f)
        {
            RuntimeErrorHandler.Flag(node, RuntimeErrorCode.DivisionByZero, this);
            return 0.0f;
        }

        return nodeType switch
        {
            IIntermediateCodeNodeType.Kind.Add => value1 + value2,
            IIntermediateCodeNodeType.Kind.Subtract => value1 - value2,
            IIntermediateCodeNodeType.Kind.Multiply => value1 * value2,
            IIntermediateCodeNodeType.Kind.Float_Divide => value1 / value2,
            _ => throw new UnreachableException("Unknown arithmetic operator for floats.")
        };
    }

    /// <summary>
    /// Handles arithmetic operations for integer operands.
    /// </summary>
    /// <param name="node">The intermediate code node representing the arithmetic operation.</param>
    /// <param name="nodeType">The type of the arithmetic operator.</param>
    /// <param name="operand1">The first operand of the arithmetic operation.</param>
    /// <param name="operand2">The second operand of the arithmetic operation.</param>
    /// <returns>
    /// The result of the arithmetic operation as an integer or float.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// Thrown if the operands are not integers.
    /// </exception>
    private object HandleIntegerMode(IIntermediateCodeNode node, IIntermediateCodeNodeType.Kind nodeType, object? operand1,
                                     object? operand2)
    {
        int value1 = operand1 switch
        {
            int value => value,
            _ => throw new UnreachableException($"{nameof(operand1)} is not arithmetic")
        };

        int value2 = operand2 switch
        {
            int value => value,
            _ => throw new UnreachableException($"{nameof(operand2)} is not arithmetic")
        };

        if (value2 == 0
            && nodeType is IIntermediateCodeNodeType.Kind.Integer_Divide
                           or IIntermediateCodeNodeType.Kind.Float_Divide
                           or IIntermediateCodeNodeType.Kind.Mod)
        {
            RuntimeErrorHandler.Flag(node, RuntimeErrorCode.DivisionByZero, this);
            return 0;
        }

        return nodeType switch
        {
            IIntermediateCodeNodeType.Kind.Add => value1 + value2,
            IIntermediateCodeNodeType.Kind.Subtract => value1 - value2,
            IIntermediateCodeNodeType.Kind.Multiply => value1 * value2,
            IIntermediateCodeNodeType.Kind.Float_Divide => (float)value1 / value2,
            IIntermediateCodeNodeType.Kind.Integer_Divide => value1 / value2,
            IIntermediateCodeNodeType.Kind.Mod => value1 % value2,
            _ => throw new UnreachableException("Unknown arithmetic operator.")
        };
    }
}
