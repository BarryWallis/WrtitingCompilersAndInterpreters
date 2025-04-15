using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrontendComponents;

namespace FrontendComponents.Pascal;

/// <summary>
/// Extracts a token based on the current character, returning an end-of-file token if the character is EOF.
/// </summary>
/// <param name="source">Provides the context needed to create tokens based on the current character.</param>
public class PascalScanner(Source source) : Scanner(source)
{
    /// <summary>
    /// Extracts a token based on the current character. It returns an end-of-file token if the current 
    /// character is EOF.
    /// </summary>
    /// <returns>Returns an EofToken or a PlaceholderToken depending on the current character.</returns>
    protected override Token ExtractToken()
    {
        char currentChar = CurrentChar;

        return currentChar switch
        {
            Source.EOF => new EofToken(Source),
            _ => new PlaceholderToken(Source),
        };
    }
}
