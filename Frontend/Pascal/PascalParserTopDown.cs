using System.Diagnostics;

using CommonInterfaces;

using FrontendComponents.Pascal.Parsers;
using FrontendComponents.Pascal.Tokens;

using Intermediate;

using Messages;

namespace FrontendComponents.Pascal;
/// <summary>
/// A class for parsing Pascal code using a top-down approach. 
/// </summary>
public class PascalParserTopDown : Parser
{
    protected static readonly PascalErrorHandler _errorHandler = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="PascalParserTopDown"/> class using the specified scanner.
    /// </summary>
    /// <remarks>This constructor sets up the parser to process Pascal source code using a top-down parsing
    /// approach. The provided <paramref name="scanner"/> is required to supply tokens for the parsing
    /// process.
    /// </remarks>
    /// <param name="scanner">The <see cref="Scanner"/> instance used to tokenize the input source code.</param>
    public PascalParserTopDown(Scanner scanner) : base(scanner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PascalParserTopDown"/> class using the specified parent parser.
    /// </summary>
    /// <param name="parent">The parent parser from which this parser inherits its scanner.</param>
    public PascalParserTopDown(PascalParserTopDown parent) : base(parent.Scanner)
    {
    }

    /// <summary>
    /// <inheritdoc cref="Parser.ErrorCount"/>
    /// </summary>
    public override int ErrorCount
    {
        get => ErrorHandler.ErrorCount;

        protected set => ErrorHandler.ErrorCount = value;
    }

    /// <summary>
    /// Parses tokens until the end of file is reached. It measures the time taken for parsing and sends a 
    /// summary message.
    /// </summary>
    public override void Parse()
    {
        PascalToken? token;
        Stopwatch stopwatch = Stopwatch.StartNew();
        IntermediateCode = IntermediateCodeFactory.CreateIntermediateCode();

        try
        {
            token = ParseProgram(stopwatch, IntermediateCode);
        }
        catch (IOException)
        {
            stopwatch.Stop();
            ErrorHandler.AbortTranslation(PascalErrorCode.IOError, this);
        }
    }

    /// <summary>
    /// Parses the Pascal program, starting with the "Begin" token, and constructs the intermediate code representation.
    /// </summary>
    /// <param name="stopwatch">The stopwatch used to measure the parsing time.</param>
    /// <param name="intermediateCode">The intermediate code structure to populate with the parsed program.</param>
    /// <returns>
    /// The last token processed, which should be the period (".") token if the program is valid.
    /// </returns>
    /// <remarks>
    /// This method ensures that the program starts with a "Begin" token and ends with a period (".") token.
    /// If the "Begin" token is missing, an error is flagged. Similarly, if the period (".") token is missing,
    /// an error is flagged. The method also updates the intermediate code's root node with the parsed program's
    /// structure and sends a summary message with parsing statistics.
    /// </remarks>
    /// <exception cref="UnreachableException">
    /// Thrown if an unexpected token type is encountered during parsing.
    /// </exception>
    private PascalToken ParseProgram(Stopwatch stopwatch, IIntermediateCode intermediateCode)
    {
        PascalToken? token = GetNextToken() as PascalToken;
        Debug.Assert(token is not null);
        IIntermediateCodeNode? rootNode = null;

        if (token.Kind == ITokenType.Kind.Begin)
        {
            StatementParser statementParser = new(this);
            rootNode = statementParser.Parse(token);
            token = CurrentToken as PascalToken;
            Debug.Assert(token is not null);
        }
        else
        {
            ErrorHandler.Flag(token, PascalErrorCode.UnexpectedToken, this);
        }

        if (token.Kind != ITokenType.Kind.Dot)
        {
            ErrorHandler.Flag(token, PascalErrorCode.MissingPeriod, this);
        }

        token = CurrentToken as PascalToken;
        Debug.Assert(token is not null);

        if (rootNode is not null)
        {
            intermediateCode.Root = rootNode;
        }

        stopwatch.Stop();
        SendMessage(new ParserSummaryMessage(token.LineNumber, ErrorCount, stopwatch.ElapsedMilliseconds / 1000f));
        return token;
    }

    /// <summary>
    /// Process the next token.
    /// </summary>
    /// <param name="token">The token to process.</param>
#pragma warning disable IDE0051 // Remove unused private members
    private void ProcessToken(Token token)
#pragma warning restore IDE0051 // Remove unused private members
    {
        if (token is not PascalErrorToken)
        {
            Debug.Assert(token is PascalToken);
            PascalToken pascalToken = (token as PascalToken)!;

            ITokenType.Kind? kind = pascalToken.Kind;
            Debug.Assert(kind is not null);

            if (kind == ITokenType.Kind.Identifier)
            {
                Debug.Assert(pascalToken.Text is not null);
                string name = pascalToken.Text.ToLowerInvariant();
                ISymbolTableEntry? entry = SymbolTableStack.Lookup(name) ?? SymbolTableStack.EnterLocal(name);

                Debug.Assert(entry is not null);
                entry.AppendLineNumber(pascalToken.LineNumber);
            }
            else if (kind == ITokenType.Kind.Error)
            {
                Debug.Assert(pascalToken.Value is PascalErrorCode);
                ErrorHandler.Flag(pascalToken, (token.Value as PascalErrorCode)!, this);
            }
        }
    }
}
