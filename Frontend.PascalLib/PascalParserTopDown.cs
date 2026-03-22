using System.Diagnostics;

using FrontendLib;

using MessageLib;

namespace Frontend.PascalLib;

/// <summary>
/// Parses Pascal source using a top-down strategy.
/// </summary>
/// <param name="theScanner">The scanner that supplies tokens to the parser.</param>
public class PascalParserTopDown(Scanner theScanner) : Parser(theScanner)
{
    private readonly PascalErrorHandler _pascalErrorHandler = new();

    /// <summary>
    /// Gets the number of syntax errors found during parsing.
    /// </summary>
    public override int ErrorCount => _pascalErrorHandler.ErrorCount;

    /// <summary>
    /// Parses source tokens until end-of-file and publishes token and parser summary messages.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when a token contains an unexpected value that cannot be processed by the parser.
    /// </exception>
    /// <remarks>
    /// Input/output failures are translated into <see cref="PascalErrorCode.IO_ERROR"/> and reported through the parser error
    /// handler.
    /// </remarks>
    public override void Parse()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        try
        {
            Token token;

            while ((token = NextToken()) is not EofToken)
            {
                Type tokenType = token.GetType();
                if (tokenType != typeof(ErrorToken))
                {
                    SendMessage(new TokenMessage(token.LineNumber,
                                                 token.Position,
                                                 tokenType,
                                                 token.Text,
                                                 token.Value));
                }
                else if (token.Value is PascalErrorCode pascalErrorCode)
                {
                    _pascalErrorHandler.Flag(token, pascalErrorCode, this);
                }
                else
                {
                    throw new InvalidOperationException($"Unexpected token value: {token.Value?.ToString()}");
                }
            }

            float elapsedTime = (float)stopwatch.Elapsed.TotalSeconds;
            SendMessage(new ParserSummaryMessage(token.LineNumber, ErrorCount, elapsedTime));
        }
        catch (System.IO.IOException)
        {
            _pascalErrorHandler.AbortTranslation(PascalErrorCode.IO_ERROR, this);
        }
    }
}
