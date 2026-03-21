using System;
using System.Collections.Generic;
using System.Text;

namespace MessageLib;

public record ParserSummaryMessage(int SourceLinesRead, int SyntaxErrors, float ElapsedTime) : Message
{
}
