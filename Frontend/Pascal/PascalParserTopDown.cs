using System.Diagnostics;

using CommonInterfaces;

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
        PascalToken? token;
        Stopwatch stopwatch = new();

        try
        {
            while ((token = GetNextToken() as PascalToken) is not EofToken)
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
    private void ProcessToken(PascalToken token)
    {
        if (token is not PascalErrorToken)
        {
            Debug.Assert(token.Text is not null);
            Debug.Assert(token.Kind is not null);
            SendMessage(new TokenMessage(token.LineNumber, token.Position,
                                         token.Kind.Value,
                                         token.Text, token.Value));
        }
        else
        {
            Debug.Assert(token.Value is not null);
            ErrorHandler.Flag(token, (PascalErrorCode)token.Value, this);
        }
    }
}
