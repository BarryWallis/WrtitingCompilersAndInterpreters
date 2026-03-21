using System;
using System.Collections.Generic;
using System.Text;

namespace MessageLib;

public record CompilerSummaryMessage(int InstructionCount, float ElapsedCodeGenerationTime) : Message
{
}
