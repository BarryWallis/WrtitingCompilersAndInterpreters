using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend;

/// <summary>
/// Represents an end-of-file token in a token stream. 
/// </summary>
public record EofToken : Token
{
    /// <summary>
    /// Initializes a new instance of the EofToken class.
    /// </summary>
    /// <param name="source">Provides the context or origin of the token being created.</param>
    public EofToken(Source source) : base(source)
    {
    }

    /// <summary>
    /// Do nothing.  Do not consume any source characters.
    /// </summary>
    protected override void Extract()
    {

    }
}
