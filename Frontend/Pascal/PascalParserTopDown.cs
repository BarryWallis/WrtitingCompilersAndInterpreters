using System.Diagnostics;

using CommonInterfaces;

using Intermediate;

using Messages;

namespace FrontendComponents.Pascal;
/// <summary>
/// A class for parsing Pascal code using a top-down approach. 
/// </summary>
/// <param name="scanner">Provides the input source for the parser to analyze the Pascal code.</param>
public class PascalParserTopDown(Scanner scanner) : Parser(scanner)
{
    protected static readonly PascalErrorHandler _errorHandler = new();

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
        Token? token;
        Stopwatch stopwatch = new();
        stopwatch.Start();

        try
        {
            while ((token = GetNextToken()) is not EofToken)
            {
                Debug.Assert(token is not null);
                ProcessToken(token);
            }

            stopwatch.Stop();
            SendMessage(new ParserSummaryMessage(token.LineNumber, ErrorCount,
                                                 stopwatch.Elapsed.TotalSeconds));
        }
        catch (IOException)
        {
            stopwatch.Stop();
            ErrorHandler.AbortTranslation(PascalErrorCode.IOError, this);
        }

    }

    /// <summary>
    /// Process the next token.
    /// </summary>
    /// <param name="token">The token to process.</param>
    private void ProcessToken(Token token)
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
