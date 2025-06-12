using System.Diagnostics;
using CommonInterfaces;
using Intermediate;

namespace FrontendComponents.Pascal.Parsers;

/// <summary>
/// Parser for Pascal REPEAT-UNTIL statements that generates intermediate code nodes.
/// </summary>
/// <remarks>
/// This parser handles the parsing of REPEAT-UNTIL control structures and generates
/// the corresponding intermediate code representation with loop and test nodes.
/// </remarks>
public class RepeatStatementParser(StatementParser parent) : StatementParser(parent)
{
    /// <summary>
    /// Parses a REPEAT-UNTIL statement and generates its intermediate code representation.
    /// </summary>
    /// <param name="token">The current token to start parsing from.</param>
    /// <returns>
    /// An <see cref="IIntermediateCodeNode"/> representing the parsed REPEAT-UNTIL statement structure,
    /// consisting of a loop node containing statement nodes and a test node.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// Thrown when an unexpected non-Pascal token is encountered during parsing.
    /// </exception>
    public override IIntermediateCodeNode? Parse(PascalToken token)
    {
        token = GetNextToken() as PascalToken ?? throw new UnreachableException($"Expecting {nameof(PascalToken)}");
        
        IIntermediateCodeNode loopNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Loop);
        IIntermediateCodeNode testNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Test);

        StatementParser statementParser = new(this);
        statementParser.ParseList(token, loopNode, ITokenType.Kind.Until, PascalErrorCode.MissingUntil);
        token = CurrentToken as PascalToken ?? throw new UnreachableException("Expected PascalToken");

        ExpressionParser expressionParser = new(this);
        _ = testNode.AddChild(expressionParser.Parse(token));
        _ = loopNode.AddChild(testNode);

        return loopNode;
    }
}
