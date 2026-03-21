using System.Diagnostics;

using FrontendLib;

using MessageLib;

namespace Frontend.PascalLib;

public class PascalParserTopDown(Scanner theScanner) : Parser(theScanner)
{
    public override int ErrorCount => 0;

    public override void Parse()
    {
        Token token;
        Stopwatch stopwatch = Stopwatch.StartNew();

        while ((token = NextToken()) is not EofToken)
        {
        }

        stopwatch.Stop();
        float elapsedTime = (float)stopwatch.Elapsed.TotalSeconds;

        SendMessage(new ParserSummaryMessage(token.LineNumber, ErrorCount, elapsedTime));
    }
}
