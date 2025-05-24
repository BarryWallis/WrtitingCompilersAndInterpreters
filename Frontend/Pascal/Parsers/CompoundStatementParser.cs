using System.Diagnostics;

using Intermediate;

namespace FrontendComponents.Pascal.Parsers;

/// <summary>
/// Parses compound statements in Pascal code and constructs an intermediate code node
/// representing the compound statement.
/// </summary>
public class CompoundStatementParser(StatementParser parent) : StatementParser(parent)
{
    /// <summary>
    /// Parses a compound statement.
    /// </summary>
    /// <param name="token">The current token to parse.</param>
    /// <returns>
    /// An <see cref="IIntermediateCodeNode"/> representing the compound statement.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// Thrown if an unexpected token type is encountered during parsing.
    /// </exception>
    public override IIntermediateCodeNode? Parse(Token token)
    {
        // Retrieve the next token, which should be the first token inside the compound statement.
        PascalToken pascalToken = GetNextToken() as PascalToken ?? throw new UnreachableException();

        // Create a compound node to represent the compound statement in the intermediate code.
        IIntermediateCodeNode compoundNode 
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Compound);

        // Use a statement parser to parse the list of statements within the compound statement.
        StatementParser statementParser = new(this);
        statementParser.ParseList(pascalToken, compoundNode, CommonInterfaces.ITokenType.Kind.End, PascalErrorCode.MissingEnd);

        return compoundNode;
    }
}
