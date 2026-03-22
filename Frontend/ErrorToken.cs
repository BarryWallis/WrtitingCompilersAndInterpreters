using System;
using System.Collections.Generic;
using System.Text;

namespace FrontendLib;

public record ErrorToken(Source TheSource) : Token(TheSource)
{
}
