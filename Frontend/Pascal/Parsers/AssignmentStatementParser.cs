using System.Diagnostics;

using CommonInterfaces;

using Intermediate;

namespace FrontendComponents.Pascal.Parsers;

/// <summary>
/// Parses assignment statements in Pascal code and constructs an intermediate code node
/// representing the assignment operation.
/// </summary>
public class AssignmentStatementParser(StatementParser parent) : StatementParser(parent)
{
    private static readonly HashSet<ITokenType.Kind> _colonEqualsSet =
    [
..      _expressionStartSet.Concat(_statementFollowSet), ITokenType.Kind.ColonEquals,
    ];

    /// <summary>
    /// Parses an assignment statement, which assigns the result of an expression to a variable.
    /// </summary>
    /// <param name="token">The current token to parse, expected to be an identifier.</param>
    /// <returns>
    /// An <see cref="IIntermediateCodeNode"/> representing the assignment statement.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// Thrown if an unexpected token type is encountered during parsing.
    /// </exception>
    public override IIntermediateCodeNode Parse(PascalToken token)
    {
        // Create an assignment node to represent the assignment operation in the intermediate code.
        IIntermediateCodeNode assignmentNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Assign);

        // Retrieve the target variable's name and look it up in the symbol table.
        string targetName = token.Text?.ToLowerInvariant() ?? throw new UnreachableException();
        ISymbolTableEntry targetId = SymbolTableStack.Lookup(targetName) ?? SymbolTableStack.EnterLocal(targetName);
        targetId.AppendLineNumber(token.LineNumber);

        // Advance to the next token.
        token = GetNextToken() as PascalToken ?? throw new UnreachableException($"Expected {nameof(PascalToken)}");

        // Create a variable node to represent the target variable in the intermediate code.
        IIntermediateCodeNode variableNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Variable);
        variableNode.SetAttribute(IIntermediateCodeKey.Key.Id, targetId);
        _ = assignmentNode.AddChild(variableNode);

        // Check for the assignment operator ":=".
        token = Synchronize(_colonEqualsSet) as PascalToken
            ?? throw new UnreachableException($"Expected {nameof(PascalToken)} with kind {ITokenType.Kind.ColonEquals}");
        if (token is PascalToken pascalToken && pascalToken.Kind == ITokenType.Kind.ColonEquals)
        {
            token = GetNextToken() as PascalToken ?? throw new UnreachableException("Expected PascalToken");
        }
        else
        {
            // Flag an error if the assignment operator is missing.
            ErrorHandler.Flag(token, PascalErrorCode.MissingColonEquals, this);
        }

        // Parse the expression on the right-hand side of the assignment.
        ExpressionParser expressionParser = new(this);
        _ = assignmentNode.AddChild(expressionParser.Parse(token));

        return assignmentNode;
    }
}
