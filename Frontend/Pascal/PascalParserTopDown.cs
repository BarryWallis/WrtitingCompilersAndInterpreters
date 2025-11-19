using MessageLib;

namespace FrontendLib.Pascal;

/// <summary>
/// A simple top-down parser for Pascal, demonstrating the parsing workflow and summary reporting.
/// </summary>
public class PascalParserTopDown(Scanner scanner) : Parser(scanner)
{
    /// <inheritdoc />
    public override void Parse()
    {
        Token token;
        DateTime startTime = DateTime.Now;
        while ((token = GetNextToken()) is not EndOfFileToken)
        {
            // Parsing logic would go here
        }

        TimeSpan elapsedTime = DateTime.Now - startTime;
        SendMessage(new ParserSummaryMessage(token.LineNumber, ErrorCount, elapsedTime));
    }

    /// <summary>
    /// For this sample, errors are not tracked and always reported as zero.
    /// </summary>
    public override int ErrorCount => 0;
}
