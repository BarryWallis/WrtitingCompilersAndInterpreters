using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using static CommonInterfaces.ITokenType;


namespace FrontendComponents.Pascal;

/// <summary>
/// Abstract base class for Pascal tokens.
/// </summary>
public abstract record PascalToken : Token
{
    public Kind? Kind { get; protected set; }

    /// <summary>
    /// Create a new Pascal token using the provided source. The token type is determined by the source 
    /// data.
    /// </summary>
    /// <param name="source"><inheritdoc/></param>
    protected PascalToken(Source source) : base(source)
    {
    }

    public char GetNextChar() => _source.GetNextChar();
}
