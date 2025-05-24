using System.Diagnostics;

using CommonInterfaces;

using Intermediate;

namespace FrontendComponents.Pascal.Parsers;

/// <summary>
/// Parses expressions in Pascal code and constructs intermediate code nodes
/// representing the expressions.
/// </summary>
public class ExpressionParser(PascalParserTopDown parent) : StatementParser(parent)
{
    /// <summary>
    /// A dictionary mapping additive operator tokens to their corresponding intermediate code node types.
    /// </summary>
    private static readonly Dictionary<ITokenType.Kind, IIntermediateCodeNodeType.Kind> _additiveOperators = new()
    {
        { ITokenType.Kind.Plus, IIntermediateCodeNodeType.Kind.Add },
        { ITokenType.Kind.Minus, IIntermediateCodeNodeType.Kind.Subtract },
        { ITokenType.Kind.Or, IIntermediateCodeNodeType.Kind.Or },
    };

    /// <summary>
    /// A dictionary mapping relational operator tokens to their corresponding intermediate code node types.
    /// </summary>
    private static readonly Dictionary<ITokenType.Kind, IIntermediateCodeNodeType.Kind> _relationalOperators = new()
    {
        { ITokenType.Kind.Equals, IIntermediateCodeNodeType.Kind.Eq },
        { ITokenType.Kind.NotEquals, IIntermediateCodeNodeType.Kind.Ne },
        { ITokenType.Kind.LessThan, IIntermediateCodeNodeType.Kind.Lt },
        { ITokenType.Kind.LessEquals, IIntermediateCodeNodeType.Kind.Le },
        { ITokenType.Kind.GreaterThan, IIntermediateCodeNodeType.Kind.Gt },
        { ITokenType.Kind.GreaterEquals, IIntermediateCodeNodeType.Kind.Ge }
    };

    /// <summary>
    /// A dictionary mapping multiplicative operator tokens to their corresponding intermediate code node types.
    /// </summary>
    private static readonly Dictionary<ITokenType.Kind, IIntermediateCodeNodeType.Kind> _multiplicativeOperators = new()
    {
        { ITokenType.Kind.Star, IIntermediateCodeNodeType.Kind.Multiply },
        { ITokenType.Kind.Slash, IIntermediateCodeNodeType.Kind.Float_Divide },
        { ITokenType.Kind.Div, IIntermediateCodeNodeType.Kind.Integer_Divide },
        { ITokenType.Kind.Mod, IIntermediateCodeNodeType.Kind.Mod },
        { ITokenType.Kind.And, IIntermediateCodeNodeType.Kind.And }
    };

    /// <summary>
    /// Parses an expression and constructs the corresponding intermediate code node.
    /// </summary>
    /// <param name="token">The current token to parse.</param>
    /// <returns>
    /// An <see cref="IIntermediateCodeNode"/> representing the parsed expression, or <see langword="null"/> if parsing fails.
    /// </returns>
    public override IIntermediateCodeNode? Parse(Token token) => ParseExpression(token);

    /// <summary>
    /// Parses an expression, which may include relational operators, and constructs the corresponding intermediate code node.
    /// </summary>
    /// <param name="token">The current token to parse.</param>
    /// <returns>
    /// An <see cref="IIntermediateCodeNode"/> representing the parsed expression, or <see langword="null"/> if parsing fails.
    /// </returns>
    private IIntermediateCodeNode? ParseExpression(Token token)
    {
        IIntermediateCodeNode? rootNode = ParseSimpleExpression(token);

        PascalToken pascalToken = CurrentToken as PascalToken ?? throw new UnreachableException();
        ITokenType.Kind tokenKind = pascalToken.Kind ?? throw new UnreachableException();

        if (_relationalOperators.TryGetValue(tokenKind, out IIntermediateCodeNodeType.Kind nodeType))
        {
            IIntermediateCodeNode operatorNode = IntermediateCodeFactory.CreateIntermediateCodeNode(nodeType);
            _ = operatorNode.AddChild(rootNode);

            token = GetNextToken();

            _ = operatorNode.AddChild(ParseSimpleExpression(token));

            rootNode = operatorNode;
        }

        return rootNode;
    }

    /// <summary>
    /// Parses a simple expression, which may include additive operators, and constructs the corresponding intermediate code node.
    /// </summary>
    /// <param name="token">The current token to parse.</param>
    /// <returns>
    /// An <see cref="IIntermediateCodeNode"/> representing the parsed simple expression, or <see langword="null"/> if parsing fails.
    /// </returns>
    private IIntermediateCodeNode? ParseSimpleExpression(Token token)
    {
        ITokenType.Kind? signType = null;

        ITokenType.Kind tokenType = (token as PascalToken)?.Kind ?? throw new UnreachableException();
        if (tokenType is ITokenType.Kind.Plus or ITokenType.Kind.Minus)
        {
            signType = tokenType;
            token = GetNextToken();
        }

        IIntermediateCodeNode? rootNode = ParseTerm(token);

        if (signType is not null and ITokenType.Kind.Minus)
        {
            IIntermediateCodeNode negateNode
                = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Negate);
            _ = negateNode.AddChild(rootNode);
            rootNode = negateNode;
        }

        token = CurrentToken;
        tokenType = (token as PascalToken)?.Kind ?? throw new UnreachableException();

        while (_additiveOperators.ContainsKey(tokenType))
        {
            _ = AddAdditiveChildren(ref tokenType, ref rootNode);
        }

        return rootNode;
    }

    /// <summary>
    /// Adds children nodes for additive operations to the intermediate code tree.
    /// </summary>
    /// <param name="tokenType">The current token type, which determines the additive operation.</param>
    /// <param name="rootNode">The root node of the current intermediate code tree.</param>
    /// <returns>
    /// The next token after processing the additive operation.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// Thrown if an unexpected token type is encountered during parsing.
    /// </exception>
    private Token AddAdditiveChildren(ref ITokenType.Kind tokenType, ref IIntermediateCodeNode? rootNode)
    {
        Token token;

        // Create an operator node for the additive operation
        IIntermediateCodeNodeType.Kind nodeType = _additiveOperators[tokenType];
        IIntermediateCodeNode operatorNode = IntermediateCodeFactory.CreateIntermediateCodeNode(nodeType);

        // Add the current root node as a child of the operator node
        _ = operatorNode.AddChild(rootNode);

        // Advance to the next token
        token = GetNextToken();

        // Parse the term and add it as a child of the operator node
        _ = operatorNode.AddChild(ParseTerm(token));

        // Update the root node to the operator node
        rootNode = operatorNode;

        // Update the token type to the current token's type
        token = CurrentToken;
        tokenType = (token as PascalToken)?.Kind ?? throw new UnreachableException();

        return token;
    }

    /// <summary>
    /// Parses a term, which may include multiplicative operators, and constructs the corresponding intermediate code node.
    /// </summary>
    /// <param name="token">The current token to parse.</param>
    /// <returns>
    /// An <see cref="IIntermediateCodeNode"/> representing the parsed term, or <see langword="null"/> if parsing fails.
    /// </returns>
    private IIntermediateCodeNode? ParseTerm(Token token)
    {
        IIntermediateCodeNode? rootNode = ParseFactor(token);

        token = CurrentToken;

        ITokenType.Kind tokenType = (token as PascalToken)?.Kind ?? throw new UnreachableException();

        while (_multiplicativeOperators.ContainsKey(tokenType))
        {
            IIntermediateCodeNodeType.Kind nodeType = _multiplicativeOperators[tokenType];
            IIntermediateCodeNode operatorNode = IntermediateCodeFactory.CreateIntermediateCodeNode(nodeType);
            _ = operatorNode.AddChild(rootNode);

            token = GetNextToken();

            _ = operatorNode.AddChild(ParseFactor(token));

            rootNode = operatorNode;
            token = CurrentToken;
            tokenType = (token as PascalToken)?.Kind ?? throw new UnreachableException();
        }

        return rootNode;
    }

    /// <summary>
    /// Parses a factor, which may be a variable, constant, or subexpression, and constructs the corresponding intermediate code node.
    /// </summary>
    /// <param name="token">The current token to parse.</param>
    /// <returns>
    /// An <see cref="IIntermediateCodeNode"/> representing the parsed factor, or <see langword="null"/> if parsing fails.
    /// </returns>
    /// <remarks>
    /// A factor can be one of the following:
    /// <list type="bullet">
    /// <item><description>A variable, represented by an identifier.</description></item>
    /// <item><description>An integer, real, or string constant.</description></item>
    /// <item><description>A subexpression enclosed in parentheses.</description></item>
    /// <item><description>A negated factor, represented by the "Not" keyword.</description></item>
    /// </list>
    /// If the factor is invalid or unexpected, an error is flagged.
    /// </remarks>
    /// <exception cref="UnreachableException">
    /// Thrown if an unexpected token type is encountered during parsing.
    /// </exception>
    private IIntermediateCodeNode? ParseFactor(Token token)
    {
        ITokenType.Kind tokenType = (token as PascalToken)?.Kind ?? throw new UnreachableException();
        IIntermediateCodeNode? rootNode = tokenType switch
        {
            ITokenType.Kind.Identifier => ParseIdentifier(token),
            ITokenType.Kind.Integer => ParseIntegerConstant(token),
            ITokenType.Kind.Real => ParseRealConstant(token),
            ITokenType.Kind.String => ParseStringConstant(token),
            ITokenType.Kind.Not => ParseNotFactor(token),
            ITokenType.Kind.LeftParen => ParseSubExpression(token),
            _ => FlagUnexpectedToken(token)
        };

        return rootNode;
    }

    /// <summary>
    /// Parses an identifier and constructs the corresponding intermediate code node.
    /// </summary>
    /// <param name="token">The current token to parse, expected to be an identifier.</param>
    /// <returns>
    /// An <see cref="IIntermediateCodeNode"/> representing the parsed identifier, or <see langword="null"/> if parsing fails.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// Thrown if the token text is null or an unexpected error occurs during parsing.
    /// </exception>
    private IIntermediateCodeNode? ParseIdentifier(Token token)
    {
        // Retrieve the name of the identifier and convert it to lowercase.
        string name = token.Text?.ToLowerInvariant() ?? throw new UnreachableException();

        // Look up the identifier in the symbol table.
        ISymbolTableEntry? id = SymbolTableStack.Lookup(name);
        if (id is null)
        {
            // Flag an error if the identifier is undefined and enter it into the symbol table.
            ErrorHandler.Flag(token, PascalErrorCode.IdentifierUndefined, this);
            id = SymbolTableStack.EnterLocal(name);
        }

        // Create a variable node to represent the identifier in the intermediate code.
        IIntermediateCodeNode rootNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Variable);
        rootNode.SetAttribute(IIntermediateCodeKey.Key.Id, id);
        id.AppendLineNumber(token.LineNumber);

        _ = GetNextToken();
        return rootNode;
    }

    /// <summary>
    /// Parses an integer constant and constructs the corresponding intermediate code node.
    /// </summary>
    /// <param name="token">The current token to parse, expected to be an integer constant.</param>
    /// <returns>
    /// An <see cref="IIntermediateCodeNode"/> representing the parsed integer constant.
    /// </returns>
    private IIntermediateCodeNode? ParseIntegerConstant(Token token)
    {
        IIntermediateCodeNode rootNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Integer_Constant);
        rootNode.SetAttribute(IIntermediateCodeKey.Key.Value, token.Value);

        _ = GetNextToken();
        return rootNode;
    }

    /// <summary>
    /// Parses a real constant and constructs the corresponding intermediate code node.
    /// </summary>
    /// <param name="token">The current token to parse, expected to be a real constant.</param>
    /// <returns>
    /// An <see cref="IIntermediateCodeNode"/> representing the parsed real constant.
    /// </returns>
    private IIntermediateCodeNode? ParseRealConstant(Token token)
    {
        IIntermediateCodeNode rootNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Real_Constant);
        rootNode.SetAttribute(IIntermediateCodeKey.Key.Value, token.Value);

        _ = GetNextToken();
        return rootNode;
    }

    /// <summary>
    /// Parses a string constant and constructs the corresponding intermediate code node.
    /// </summary>
    /// <param name="token">The current token to parse, expected to be a string constant.</param>
    /// <returns>
    /// An <see cref="IIntermediateCodeNode"/> representing the parsed string constant.
    /// </returns>
    private IIntermediateCodeNode? ParseStringConstant(Token token)
    {
        string value = token.Value?.ToString() ?? throw new UnreachableException();

        IIntermediateCodeNode rootNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.String_Constant);
        rootNode.SetAttribute(IIntermediateCodeKey.Key.Value, value);

        _ = GetNextToken();
        return rootNode;
    }

    /// <summary>
    /// Parses a negated factor (using the "Not" keyword) and constructs the corresponding intermediate code node.
    /// </summary>
    /// <param name="token">The current token to parse, expected to be the "Not" keyword.</param>
    /// <returns>
    /// An <see cref="IIntermediateCodeNode"/> representing the parsed negated factor.
    /// </returns>
    private IIntermediateCodeNode? ParseNotFactor(Token token)
    {
        Debug.Assert(token is PascalToken { Kind: ITokenType.Kind.Not });

        _ = GetNextToken();

        IIntermediateCodeNode rootNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind.Not);

        _ = rootNode.AddChild(ParseFactor(token));
        return rootNode;
    }

    /// <summary>
    /// Parses a subexpression enclosed in parentheses and constructs the corresponding intermediate code node.
    /// </summary>
    /// <param name="token">The current token to parse, expected to be a left parenthesis.</param>
    /// <returns>
    /// An <see cref="IIntermediateCodeNode"/> representing the parsed subexpression.
    /// </returns>
    private IIntermediateCodeNode? ParseSubExpression(Token token)
    {
        Debug.Assert(token is PascalToken { Kind: ITokenType.Kind.LeftParen });

        token = GetNextToken();

        IIntermediateCodeNode? rootNode = ParseExpression(token);

        PascalToken pascalToken = CurrentToken as PascalToken ?? throw new UnreachableException();
        if (pascalToken.Kind == ITokenType.Kind.RightParen)
        {
            _ = GetNextToken();
        }
        else
        {
            ErrorHandler.Flag(token, PascalErrorCode.MissingRightParen, this);
        }

        return rootNode;
    }

    /// <summary>
    /// Flags an unexpected token as an error and returns null.
    /// </summary>
    /// <param name="token">The unexpected token to flag.</param>
    /// <returns>
    /// Always returns <see langword="null"/>.
    /// </returns>
    private IIntermediateCodeNode? FlagUnexpectedToken(Token token)
    {
        ErrorHandler.Flag(token, PascalErrorCode.UnexpectedToken, this);
        return null;
    }
}
