using System;
using System.Collections.Generic;
using System.Text;

namespace MessageLib;

public record InterpreterSummaryMessage(int ExecutionCount, int RuntimeErrors, float ElapsedTime) : Message
{
}
