using System.Diagnostics;

using CommonInterfaces;

using Intermediate;

namespace FrontendComponents.Pascal.Parsers;

/// <summary>
/// Parser implementation for Pascal FOR statement constructs. Handles parsing and intermediate code generation
/// for both ascending (TO) and descending (DOWNTO) FOR loops.
/// </summary>
/// <remarks>
/// The parser follows the Pascal FOR statement syntax:
/// FOR control-variable := initial-value (TO | DOWNTO) final-value DO statement
/// 
/// The generated intermediate code structure consists of:
/// 1. Initial assignment of the control variable
/// 2. Loop structure with test condition
/// 3. Loop body
/// 4. Step assignment (increment/decrement)
/// </remarks>
/// <param name="parent">The parent statement parser that instantiated this parser.</param>
public class ForStatementParser(StatementParser parent) : StatementParser(parent)
{
    /// <summary>
    /// Set of token kinds that are valid before and including the TO/DOWNTO keywords.
    /// Used for synchronization during error recovery.
    /// </summary>
    private static readonly HashSet<ITokenType.Kind> _toDowntoSet =
        [.. _statementStartSet.Concat(_statementFollowSet)
                                          .Append(ITokenType.Kind.To)
                                          .Append(ITokenType.Kind.Downto)];

    /// <summary>
    /// Set of token kinds that are valid before and including the DO keyword.
    /// Used for synchronization during error recovery.
    /// </summary>
    private static readonly HashSet<ITokenType.Kind> _doSet = 
        [.. _statementStartSet.Concat(_statementFollowSet)
                                         .Append(ITokenType.Kind.Do)];

    /// <summary>
    /// Stores the direction of the FOR loop (TO for ascending, DOWNTO for descending).
    /// Determines how the control variable is modified in each iteration.
    /// </summary>
    private ITokenType.Kind _direction;

    /// <summary>
    /// Parses a Pascal FOR statement and generates the corresponding intermediate code.
    /// </summary>
    /// <param name="token">The current token to begin parsing from.</param>
    /// <returns>An intermediate code node representing the FOR statement structure, or null if parsing fails.</returns>
    /// <exception cref="UnreachableException">Thrown when encountering unexpected token types during parsing.</exception>
    public override IIntermediateCodeNode? Parse(PascalToken token)
    {
        token = GetNextToken() as PascalToken ?? throw new UnreachableException($"Expected {nameof(PascalToken)}");
        PascalToken targetToken = token;

        IIntermediateCodeNode compoundNode = CreateBaseNodes(out IIntermediateCodeNode? loopNode, out IIntermediateCodeNode? testNode);
        IIntermediateCodeNode initAssignmentNode = ParseInitialAssignment(token, targetToken);
        
        _ = compoundNode.AddChild(initAssignmentNode);
        _ = compoundNode.AddChild(loopNode);

        (IIntermediateCodeNode controlVariableNode, IIntermediateCodeNode relationalOperatorNode) = ParseDirectionAndTest(token, initAssignmentNode);
        
        _ = testNode.AddChild(relationalOperatorNode);
        _ = loopNode.AddChild(testNode);

        token = ParseDoStatement(token);
        _ = loopNode.AddChild(ParseLoopBody(token));

        IIntermediateCodeNode nextAssignmentNode = CreateStepAssignment(targetToken, controlVariableNode);
        _ = loopNode.AddChild(nextAssignmentNode);

        return compoundNode;
    }

    /// <summary>
    /// Creates the base nodes required for a FOR statement structure.
    /// </summary>
    /// <param name="loopNode">The output loop node that will contain the FOR statement body.</param>
    /// <param name="testNode">The output test node that will contain the loop condition.</param>
    /// <returns>A compound node that will serve as the root of the FOR statement structure.</returns>
    private static IIntermediateCodeNode CreateBaseNodes(out IIntermediateCodeNode loopNode, out IIntermediateCodeNode testNode)
    {
        IIntermediateCodeNode compoundNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Compound);
        loopNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Loop);
        testNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Test);
        return compoundNode;
    }

    /// <summary>
    /// Parses the initial assignment statement of the FOR loop.
    /// </summary>
    /// <param name="token">The current token to parse from.</param>
    /// <param name="targetToken">The token used for line number tracking.</param>
    /// <returns>An intermediate code node representing the initial assignment.</returns>
    /// <exception cref="UnreachableException">Thrown when the assignment statement is invalid.</exception>
    private IIntermediateCodeNode ParseInitialAssignment(PascalToken token, PascalToken targetToken)
    {
        AssignmentStatementParser assignmentParser = new(this);
        IIntermediateCodeNode initAssignmentNode = assignmentParser.Parse(token)
            ?? throw new UnreachableException("Expected a valid assignment statement node");
        
        SetLineNumber(initAssignmentNode, targetToken);
        return initAssignmentNode;
    }

    /// <summary>
    /// Parses the direction (TO/DOWNTO) and test condition of the FOR statement.
    /// </summary>
    /// <param name="token">The current token to parse from.</param>
    /// <param name="initAssignmentNode">The initial assignment node containing the control variable.</param>
    /// <returns>A tuple containing the control variable node and the relational operator node.</returns>
    /// <exception cref="UnreachableException">Thrown when encountering unexpected token types.</exception>
    private (IIntermediateCodeNode controlVariableNode, IIntermediateCodeNode relationalOperatorNode) 
        ParseDirectionAndTest(PascalToken token, IIntermediateCodeNode initAssignmentNode)
    {
        token = Synchronize(_toDowntoSet) as PascalToken 
            ?? throw new UnreachableException($"Expected {nameof(PascalToken)} with kind To or Downto");
        ITokenType.Kind direction = token.Kind ?? throw new UnreachableException("Expected a valid token kind");

        if (direction is ITokenType.Kind.To or ITokenType.Kind.Downto)
        {
            token = GetNextToken() as PascalToken ?? throw new UnreachableException($"Expected {nameof(PascalToken)}");
        }
        else
        {
            direction = ITokenType.Kind.To;
            ErrorHandler.Flag(token, PascalErrorCode.MissingToDownto, this);
        }

        _direction = direction;

        IIntermediateCodeNode relationalOperatorNode = 
            IntermediateCodeFactory.CreateIntermediateCodeNode(direction == ITokenType.Kind.To 
                ? IIntermediateCodeNodeType.Kind.Gt 
                : IIntermediateCodeNodeType.Kind.Lt);

        IIntermediateCodeNode controlVariableNode = initAssignmentNode.Children[0];
        _ = relationalOperatorNode.AddChild(controlVariableNode.Clone() as IIntermediateCodeNode); 

        ExpressionParser expressionParser = new(this);
        _ = relationalOperatorNode.AddChild(expressionParser.Parse(token));

        return (controlVariableNode, relationalOperatorNode);
    }

    /// <summary>
    /// Parses the DO keyword and handles any related syntax errors.
    /// </summary>
    /// <param name="token">The current token to parse from.</param>
    /// <returns>The next token after processing the DO keyword.</returns>
    /// <exception cref="UnreachableException">Thrown when encountering unexpected token types.</exception>
    private PascalToken ParseDoStatement(PascalToken token)
    {
        token = Synchronize(_doSet) as PascalToken ?? throw new UnreachableException($"Expected {nameof(PascalToken)} with kind Do");
        if (token.Kind == ITokenType.Kind.Do)
        {
            token = GetNextToken() as PascalToken ?? throw new UnreachableException($"Expected {nameof(PascalToken)}");
        }
        else
        {
            ErrorHandler.Flag(token, PascalErrorCode.MissingDo, this);
        }
        return token;
    }

    /// <summary>
    /// Parses the body of the FOR loop.
    /// </summary>
    /// <param name="token">The current token to parse from.</param>
    /// <returns>An intermediate code node representing the loop body.</returns>
    /// <exception cref="UnreachableException">Thrown when the statement is invalid.</exception>
    private IIntermediateCodeNode ParseLoopBody(PascalToken token)
    {
        StatementParser statementParser = new(this);
        return statementParser.Parse(token) ?? throw new UnreachableException("Expected a valid statement node");
    }

    /// <summary>
    /// Creates the step assignment node that increments or decrements the loop control variable.
    /// </summary>
    /// <param name="targetToken">The token used for line number tracking.</param>
    /// <param name="controlVariableNode">The control variable node to be modified.</param>
    /// <returns>An intermediate code node representing the step assignment.</returns>
    private IIntermediateCodeNode CreateStepAssignment(PascalToken targetToken, IIntermediateCodeNode controlVariableNode)
    {
        IIntermediateCodeNode nextAssignmentNode = 
            IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Assign);
        _ = nextAssignmentNode.AddChild(controlVariableNode.Clone() as IIntermediateCodeNode);

        IIntermediateCodeNode arithmeticOperatorNode = _direction == ITokenType.Kind.To
            ? IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Add)
            : IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Subtract);

        _ = arithmeticOperatorNode.AddChild(controlVariableNode.Clone() as IIntermediateCodeNode);
        _ = nextAssignmentNode.AddChild(arithmeticOperatorNode);
        IIntermediateCodeNode constantOneNode 
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Integer_Constant);
        constantOneNode.SetAttribute(IIntermediateCodeKey.Key.Value, 1);
        _ = arithmeticOperatorNode.AddChild(constantOneNode);
        
        SetLineNumber(nextAssignmentNode, targetToken);
        return nextAssignmentNode;
    }
}
