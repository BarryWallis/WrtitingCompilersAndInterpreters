using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

using CommonInterfaces;

using Intermediate;

namespace FrontendComponents.Pascal.Parsers;

/// <summary>
/// Parses Pascal statements and constructs intermediate code nodes.
/// </summary>
public class StatementParser(PascalParserTopDown parent) : PascalParserTopDown(parent)
{
    /// <summary>
    /// Parses a Pascal statement and returns the corresponding intermediate code node.
    /// </summary>
    /// <param name="token">The current token to parse.</param>
    /// <returns>
    /// An <see cref="IIntermediateCodeNode"/> representing the parsed statement, or <see langword="null"/> if parsing fails.
    /// </returns>
    public virtual IIntermediateCodeNode? Parse(Token token)
    {
        IIntermediateCodeNode? statementNode;
        Debug.Assert(token is PascalToken);
        switch ((token as PascalToken)!.Kind)
        {
            case ITokenType.Kind.Begin:
                CompoundStatementParser compoundParser = new(this);
                statementNode = compoundParser.Parse(token);
                break;
            case ITokenType.Kind.Identifier:
                AssignmentStatementParser assignmentStatementParser = new(this);
                statementNode = assignmentStatementParser.Parse(token);
                break;
            default:
                statementNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.No_Op);
                break;
        }

        SetLineNumber(statementNode, token);
        return statementNode;
    }

    /// <summary>
    /// Sets the line number attribute for the given intermediate code node.
    /// </summary>
    /// <param name="node">The intermediate code node to update.</param>
    /// <param name="token">The token containing the line number information.</param>
    protected static void SetLineNumber(IIntermediateCodeNode? node, Token token)
        => node?.SetAttribute(IIntermediateCodeKey.Key.Line, token.LineNumber);

    /// <summary>
    /// Parses a list of statements or constructs, adding each parsed node as a child to the specified parent node.
    /// </summary>
    /// <remarks>
    /// This method iterates through tokens, parsing each one into an intermediate code node and
    /// adding it to the parent node. It handles semicolon-separated lists and flags errors for missing or unexpected
    /// tokens.
    /// </remarks>
    /// <param name="token">The current token to be parsed.</param>
    /// <param name="parentNode">The parent node to which parsed child nodes will be added.</param>
    /// <param name="terminator">The token type that signifies the end of the list.</param>
    /// <param name="errorCode">The error code to flag if the terminator is missing.</param>
    /// <exception cref="UnreachableException">
    /// Thrown if a non-Pascal token is encountered during parsing, indicating an internal error.
    /// </exception>
    public void ParseList(PascalToken token, IIntermediateCodeNode parentNode, ITokenType.Kind terminator, PascalErrorCode errorCode)
    {
        while (token as Token is not EofToken && token.Kind != terminator)
        {
            IIntermediateCodeNode statementNode = Parse(token) ?? throw new UnreachableException();
            _ = parentNode.AddChild(statementNode);
            token = CurrentToken as PascalToken ?? throw new UnreachableException();
            ITokenType.Kind tokenType = token.Kind ?? throw new UnreachableException();

            if (tokenType == ITokenType.Kind.Semicolon)
            {
                token = GetNextToken() as PascalToken ?? throw new UnreachableException();
            }
            else if (tokenType == ITokenType.Kind.Identifier)
            {
                ErrorHandler.Flag(token, PascalErrorCode.MissingSemicolon, this);
            }
            else if (tokenType != terminator)
            {
                ErrorHandler.Flag(token, PascalErrorCode.UnexpectedToken, this);
                token = GetNextToken() as PascalToken ?? throw new UnreachableException();
            }
        }

        if (token.Kind == terminator)
        {
            _ = GetNextToken() as PascalToken ?? throw new UnreachableException();
        }
        else
        {
            ErrorHandler.Flag(token, errorCode, this);
        }
    }
}
