using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrontendComponents.Pascal;

namespace FrontendComponents;

/// <summary>
/// Creates a parser for the specified programming language and type. 
/// </summary>
public static class FrontendFactory
{
    /// <summary>
    /// Creates a parser based on the specified programming language and parsing type.
    /// </summary>
    /// <param name="language">
    /// Specifies the programming language for which the parser is being created.
    /// </param>
    /// <param name="type">Indicates the parsing strategy to be used for the specified language.</param>
    /// <param name="source">Provides the source code that the parser will analyze.</param>
    /// <returns>Returns an instance of a parser configured for the given language and type.</returns>
    /// <exception cref="UnsupportedLanguageException">
    /// Thrown when the specified language is not supported.
    /// </exception>
    /// <exception cref="UnsupportedParserTypeException">
    /// Thrown when the specified parsing type is not supported.
    /// </exception>
    /// <exception cref="UnreachableException">
    /// Thrown if the code execution reaches an unexpected state.
    /// </exception>
    public static Parser CreateParser(string language, string type, Source source)
    {
        if (language.Equals("pascal", StringComparison.OrdinalIgnoreCase)
            && type.Equals("top-down", StringComparison.OrdinalIgnoreCase))
        {
            return new PascalParserTopDown(new PascalScanner(source));
        }
        else if (!language.Equals("pascal", StringComparison.OrdinalIgnoreCase))
        {
            throw new UnsupportedLanguageException(language);
        }
        else if (!type.Equals("top-down", StringComparison.OrdinalIgnoreCase))
        {
            throw new UnsupportedParserTypeException(type);
        }
        else
        {
            throw new UnreachableException("This code should never be reached.");
        }
    }
}
