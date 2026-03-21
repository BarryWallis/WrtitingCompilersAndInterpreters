using System;

using Frontend.PascalLib;
using FrontendLib;

namespace Frontend.CompositionLib;

/// <summary>
/// Composes frontend abstractions with concrete language implementations.
/// </summary>
public static class FrontendFactory
{
    /// <summary>
    /// Creates a parser for a supported language and parser type.
    /// </summary>
    /// <param name="language">The name of the source language (for example, "Pascal").</param>
    /// <param name="type">The parser type (for example, "top-down").</param>
    /// <param name="source">The source object.</param>
    /// <returns>The parser.</returns>
    /// <exception cref="Exception">Thrown when the language or parser type is invalid.</exception>
    public static Parser CreateParser(string language, string type, Source source)
    {
        if (string.Equals(language, "Pascal", StringComparison.OrdinalIgnoreCase) &&
            string.Equals(type, "top-down", StringComparison.OrdinalIgnoreCase))
        {
            Scanner scanner = new PascalScanner(source);
            return new PascalParserTopDown(scanner);
        }

        if (!string.Equals(language, "Pascal", StringComparison.OrdinalIgnoreCase))
        {
            throw new Exception($"Parser factory: Invalid language '{language}'");
        }

        throw new Exception($"Parser factory: Invalid type '{type}'");
    }
}
