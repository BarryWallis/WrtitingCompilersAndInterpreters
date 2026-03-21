using System;
using System.Collections.Generic;
using System.Text;

using FrontendLib;

namespace Frontend.PascalLib;

/// <summary>
/// Scans Pascal source text and produces tokens.
/// </summary>
/// <param name="theSource">The source to scan.</param>
public class PascalScanner(Source theSource) : Scanner(theSource)
{
    /// <summary>
    /// Extracts the next token from the source stream.
    /// </summary>
    /// <returns>An <see cref="EofToken"/> at end of input; otherwise a <see cref="Token"/>.</returns>
    protected override Token ExtractToken()
    {
        char currentChar = CurrentChar();

        // Construct the next token. The current character determines the token type.
        return currentChar == Source.EOF ? new EofToken(source) : new Token(source);
    }
}
