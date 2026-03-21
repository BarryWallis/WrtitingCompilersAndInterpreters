using System;
using System.Collections.Generic;
using System.Text;

namespace FrontendLib;

public record EofToken : Token
{
    public EofToken(Source theSource) : base(theSource)
    {
    }
}
