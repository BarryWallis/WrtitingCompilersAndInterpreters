﻿

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Diagnostics.Metrics;

using CommonInterfaces;

namespace Frontend;

/// <summary>
/// Abstract record representing a token with properties for type, text, and value. Initializes with source 
/// data and extracts token information for a single character by default.
/// </summary>
public abstract record Token
{
    protected ITokenType? Type = null;
    protected Source Source;

    public int LineNumber { get; }
    public int Position { get; }
    public string? Text { get; protected set; } = null;
    public object? Value { get; protected set; } = null;

    /// <summary>
    /// Initializes a Token object using a provided source, extracting relevant information from it.
    /// </summary>
    /// <param name="source">
    /// The input provides the necessary data to set the token's line number and 
    /// position.
    /// </param>
    public Token(Source source)
    {
        Source = source;
        LineNumber = source.LineNumber;
        Position = source.Position ?? 0;
        Extract();
    }

    /// <summary>
    /// Extracts the current character as a string and assigns it to Text. 
    /// Resets Value to null and advances to the next character.
    /// </summary>
    /// <remarks>
    /// Default method to extract only one-character tokens from the source. Subclasses can override this 
    /// method to construct language-specific tokens.  After extracting the token, the current source line 
    /// position will be one beyond the last token character.
    /// </remarks>
    protected virtual void Extract()
    {
        Text = char.ToString(CurrentChar());
        Value = null;
        NextChar();
    }

    /// <summary>
    /// Advances to the next character in the source. 
    /// </summary>
    protected void NextChar() => Source.GetNextChar();

    /// <summary>
    /// Retrieves the current character from the source.
    /// </summary>
    /// <returns>Returns the character currently being processed.</returns>
    protected char CurrentChar() => Source.GetCurrentChar();

    /// <summary>
    /// Retrieves the next character from the source without advancing the position.
    /// </summary>
    /// <returns>Returns the next character in the source.</returns>
    protected char PeekChar() => Source.PeekChar();
}
