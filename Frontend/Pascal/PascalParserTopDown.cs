using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrontendComponents;
using Messages;

namespace FrontendComponents.Pascal;

/// <summary>
/// A class for parsing Pascal code using a top-down approach. 
/// </summary>
/// <param name="scanner">Provides the input source for the parser to analyze the Pascal code.</param>
public class PascalParserTopDown(Scanner scanner) : Parser(scanner)
{
    private int _errorCount;

    /// <summary>
    /// <inheritdoc cref="Parser.ErrorCount"/>
    /// </summary>
    public override int ErrorCount
    {
        get => _errorCount;
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
        while ((token = GetNextToken()) is not EofToken)
        {
            // Do nothing
        }

        stopwatch.Stop();
        SendMessage(new ParserSummaryMessage(token.LineNumber, ErrorCount, stopwatch.Elapsed.TotalSeconds));
    }
}
