using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontendComponents.Pascal;
public record ErrorToken : Token
{
    public ErrorToken(Source source) : base(source)
    {
    }
}
