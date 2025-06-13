using System.Diagnostics;

using CommonInterfaces;

using Intermediate;

namespace FrontendComponents.Pascal.Parsers;

/// <summary>
/// Parser for Pascal IF statements that constructs corresponding intermediate code nodes.
/// </summary>
/// <remarks>
/// This parser handles the IF-THEN-ELSE construct of Pascal, creating an intermediate code tree
/// that represents the conditional statement structure.
/// </remarks>
public class IfStatementParser(StatementParser parent) : StatementParser(parent)
{
    /// <summary>
    /// Set of token types that can follow a THEN keyword in an IF statement.
    /// Includes statement start tokens, statement follow tokens, and the THEN token.
    /// </summary>
    private static readonly HashSet<ITokenType.Kind> _thenSet =
    [
     .. _statementStartSet.Concat(_statementFollowSet).Append(ITokenType.Kind.Then)
    ];

     /// <summary>
    /// Parses a Pascal IF statement and constructs its intermediate code representation.
    /// </summary>
    /// <param name="token">The current token to start parsing from.</param>
    /// <returns>An intermediate code node representing the IF statement structure, or null if parsing fails.</returns>
    /// <exception cref="UnreachableException">Thrown when an unexpected token type is encountered during parsing.</exception>
    /// <remarks>
    /// The method constructs an IF node with three potential children:
    /// 1. The condition expression
    /// 2. The THEN statement block
    /// 3. The ELSE statement block (if present)
    /// 
    /// If the THEN keyword is missing, a syntax error is flagged but parsing continues.
    /// </remarks>
    public override IIntermediateCodeNode? Parse(PascalToken token)
    {
        token = GetNextToken() as PascalToken ?? throw new UnreachableException($"Expecting {nameof(PascalToken)}");

        IIntermediateCodeNode ifNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.If);

        ExpressionParser expressionParser = new(this);
        _ = ifNode.AddChild(expressionParser.Parse(token));

        token = Synchronize(_thenSet) as PascalToken ?? throw new UnreachableException($"Expected {nameof(PascalToken)}");
        if (token.Kind == ITokenType.Kind.Then)
        {
            token = GetNextToken() as PascalToken ?? throw new UnreachableException($"Expecting {nameof(PascalToken)}");
        }
        else
        {
            ErrorHandler.Flag(token, PascalErrorCode.MissingThen, this);
        }

        StatementParser statementParser = new(this);
        _ = ifNode.AddChild(statementParser.Parse(token));
        token = CurrentToken as PascalToken ?? throw new UnreachableException("Expected PascalToken");

        if (token.Kind == ITokenType.Kind.Else)
        {
            token = GetNextToken() as PascalToken ?? throw new UnreachableException($"Expecting {nameof(PascalToken)}");
            _ = ifNode.AddChild(statementParser.Parse(token));
        }

        return ifNode;
    }
}
