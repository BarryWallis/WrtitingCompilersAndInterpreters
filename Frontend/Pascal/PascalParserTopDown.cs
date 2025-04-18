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
    private int _errorCount;

    protected static readonly PascalErrorHandler errorHandler = new();

    /// <summary>
    /// <inheritdoc cref="Parser.ErrorCount"/>
    /// </summary>
    public override int ErrorCount
    {
        get => ErrorHandler.ErrorCount;
        protected set => _errorCount = value;
    }

    /// <summary>
    /// Parses tokens until the end of file is reached. It measures the time taken for parsing and sends a 
    /// summary message.
    /// </summary>
    public override void Parse()
    {
        Token token;
        Stopwatch stopwatch = new();
        stopwatch.Start();

        try
        {
            while ((token = GetNextToken()) is not EofToken)
            {
                if (token is not ErrorToken)
                {
                    Debug.Assert(token.Text is not null);
                    SendMessage(new TokenMessage(token.LineNumber, token.Position, TokenType.Placeholder, 
                                                 token.Text, token.Value));
                }
                else
                {
                    Debug.Assert(token.Value is not null);
                    ErrorHandler.Flag(token, (PascalErrorCode)token.Value, this);
                }
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
}
