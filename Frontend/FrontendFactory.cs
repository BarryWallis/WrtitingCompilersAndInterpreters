using System.Diagnostics;

using FrontendLib.Pascal;

namespace FrontendLib;

/// <summary>
/// Factory for creating language-specific parsers and scanners.
/// </summary>
public class FrontendFactory
{
    /// <summary>
    /// Creates a parser for a given language and type.
    /// </summary>
    /// <param name="language">The source language (e.g., "Pascal").</param>
    /// <param name="type">The parser type (e.g., "top-down").</param>
    /// <param name="source">The source to read from.</param>
    /// <returns>An initialized <see cref="Parser"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when the language or type is unsupported.</exception>
    public static Parser CreateParser(string language, string type, Source source)
    {
        if (language.Equals("Pascal", StringComparison.OrdinalIgnoreCase))
        {
            if (type.Equals("top-down", StringComparison.OrdinalIgnoreCase))
            {
                Scanner scanner = new Pascal.PascalScanner(source);
                return new PascalParserTopDown(scanner);
            }
            else 
            {
                throw new ArgumentException($"Unsupported parser type: {type}");
            }
        }
        else
        {
            throw new ArgumentException($"Unsupported parser language: {language}");
        }

        throw new UnreachableException();
    }
}
