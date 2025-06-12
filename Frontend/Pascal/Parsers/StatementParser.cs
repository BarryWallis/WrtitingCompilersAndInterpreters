using System.Diagnostics;

using CommonInterfaces;

using Intermediate;

namespace FrontendComponents.Pascal.Parsers;

/// <summary>
/// Parses Pascal statements and constructs intermediate code nodes.
/// </summary>
public class StatementParser(PascalParserTopDown parent) : PascalParserTopDown(parent)
{
    protected static readonly HashSet<ITokenType.Kind> _statementStartSet =
    [
        ITokenType.Kind.Begin,
        ITokenType.Kind.Case,
        ITokenType.Kind.For,
        ITokenType.Kind.If,
        ITokenType.Kind.Repeat,
        ITokenType.Kind.While,
        ITokenType.Kind.Identifier,
        ITokenType.Kind.Semicolon,
    ];

    protected static readonly HashSet<ITokenType.Kind> _statementFollowSet =
    [
        ITokenType.Kind.Semicolon,
        ITokenType.Kind.End,
        ITokenType.Kind.Else,
        ITokenType.Kind.Until,
        ITokenType.Kind.Dot,
    ];

    protected static readonly HashSet<ITokenType.Kind> _expressionStartSet =
    [
        ITokenType.Kind.Plus,
        ITokenType.Kind.Minus,
        ITokenType.Kind.Identifier,
        ITokenType.Kind.Integer,
        ITokenType.Kind.Real,
        ITokenType.Kind.String,
        ITokenType.Kind.Not,
        ITokenType.Kind.LeftParen,
    ];

    /// <summary>
    /// Parses a Pascal statement and returns the corresponding intermediate code node.
    /// </summary>
    /// <param name="token">The current token to parse.</param>
    /// <returns>
    /// An <see cref="IIntermediateCodeNode"/> representing the parsed statement, or <see langword="null"/> if parsing fails.
    /// </returns>
    public virtual IIntermediateCodeNode? Parse(PascalToken token)
    {
        IIntermediateCodeNode? statementNode = null;
        switch (token!.Kind)
        {
            case ITokenType.Kind.Begin:
                CompoundStatementParser compoundParser = new(this);
                statementNode = compoundParser.Parse(token);
                break;
            case ITokenType.Kind.Identifier:
                AssignmentStatementParser assignmentStatementParser = new(this);
                statementNode = assignmentStatementParser.Parse(token);
                break;
            case ITokenType.Kind.Repeat:
                RepeatStatementParser repeatParser = new(this);
                statementNode = repeatParser.Parse(token);
                break;
            case ITokenType.Kind.While:
                WhileStatementParser whileParser = new(this);
                statementNode = whileParser.Parse(token);
                break;
            case ITokenType.Kind.For:
                ForStatementParser forParser = new(this);
                statementNode = forParser.Parse(token);
                break;
            case ITokenType.Kind.If:
                IfStatementParser ifParser = new(this);
                statementNode = ifParser.Parse(token);
                break;
            case ITokenType.Kind.Case:
                CaseStatementParser caseParser = new(this);
                statementNode = caseParser.Parse(token);
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
        HashSet<ITokenType.Kind> terminatorSet = [.. _statementStartSet, terminator];

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
            else if (_statementStartSet.Contains(tokenType))
            {
                ErrorHandler.Flag(token, PascalErrorCode.MissingSemicolon, this);
            }

            token = Synchronize(terminatorSet) as PascalToken ?? throw new UnreachableException($"Expected {nameof(PascalToken)}");
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
