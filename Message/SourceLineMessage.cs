using System;
using System.Collections.Generic;
using System.Text;

namespace MessageLib;

public record SourceLineMessage(int LineNumber, string Line) : Message;
