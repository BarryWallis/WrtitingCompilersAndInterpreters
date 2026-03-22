using System.Collections.Generic;

namespace Frontend.PascalLib;

/// <summary>
/// Pascal token kinds.
/// </summary>
public enum PascalTokenType
{
    // Reserved words.
    And,
    Array,
    Begin,
    Case,
    Const,
    Div,
    Do,
    Downto,
    Else,
    End,
    File,
    For,
    Function,
    Goto,
    If,
    In,
    Label,
    Mod,
    Nil,
    Not,
    Of,
    Or,
    Packed,
    Procedure,
    Program,
    Record,
    Repeat,
    Set,
    Then,
    To,
    Type,
    Until,
    Var,
    While,
    With,

    // Special symbols.
    Plus,
    Minus,
    Star,
    Slash,
    ColonEquals,
    Dot,
    Comma,
    Semicolon,
    Colon,
    Quote,
    Equals,
    NotEquals,
    LessThan,
    LessEquals,
    GreaterEquals,
    GreaterThan,
    LeftParen,
    RightParen,
    LeftBracket,
    RightBracket,
    LeftBrace,
    RightBrace,
    UpArrow,
    DotDot,

    Identifier,
    Integer,
    Real,
    String,
    Error,
    EndOfFile,
}

/// <summary>
/// Provides Pascal token metadata and lookups.
/// </summary>
public static class PascalTokenTypes
{
    /// <summary>
    /// Gets the set of lower-cased Pascal reserved words.
    /// </summary>
    public static ISet<string> ReservedWords { get; } = new HashSet<string>
    {
        "and", "array", "begin", "case", "const", "div", "do", "downto", "else", "end",
        "file", "for", "function", "goto", "if", "in", "label", "mod", "nil", "not",
        "of", "or", "packed", "procedure", "program", "record", "repeat", "set",
        "then", "to", "type", "until", "var", "while", "with",
    };

    /// <summary>
    /// Gets a mapping from special symbol text to Pascal token type.
    /// </summary>
    public static IReadOnlyDictionary<string, PascalTokenType> SpecialSymbols { get; } =
        new Dictionary<string, PascalTokenType>
        {
            ["+"] = PascalTokenType.Plus,
            ["-"] = PascalTokenType.Minus,
            ["*"] = PascalTokenType.Star,
            ["/"] = PascalTokenType.Slash,
            [":="] = PascalTokenType.ColonEquals,
            ["."] = PascalTokenType.Dot,
            [","] = PascalTokenType.Comma,
            [";"] = PascalTokenType.Semicolon,
            [":"] = PascalTokenType.Colon,
            ["'"] = PascalTokenType.Quote,
            ["="] = PascalTokenType.Equals,
            ["<>"] = PascalTokenType.NotEquals,
            ["<"] = PascalTokenType.LessThan,
            ["<="] = PascalTokenType.LessEquals,
            [">="] = PascalTokenType.GreaterEquals,
            [">"] = PascalTokenType.GreaterThan,
            ["("] = PascalTokenType.LeftParen,
            [")"] = PascalTokenType.RightParen,
            ["["] = PascalTokenType.LeftBracket,
            ["]"] = PascalTokenType.RightBracket,
            ["{"] = PascalTokenType.LeftBrace,
            ["}"] = PascalTokenType.RightBrace,
            ["^"] = PascalTokenType.UpArrow,
            [".."] = PascalTokenType.DotDot,
        };

    /// <summary>
    /// Returns the source text for a token type.
    /// </summary>
    /// <param name="tokenType">The token type.</param>
    /// <returns>The token text representation.</returns>
    public static string GetText(PascalTokenType tokenType) => tokenType switch
    {
        PascalTokenType.Plus => "+",
        PascalTokenType.Minus => "-",
        PascalTokenType.Star => "*",
        PascalTokenType.Slash => "/",
        PascalTokenType.ColonEquals => ":=",
        PascalTokenType.Dot => ".",
        PascalTokenType.Comma => ",",
        PascalTokenType.Semicolon => ";",
        PascalTokenType.Colon => ":",
        PascalTokenType.Quote => "'",
        PascalTokenType.Equals => "=",
        PascalTokenType.NotEquals => "<>",
        PascalTokenType.LessThan => "<",
        PascalTokenType.LessEquals => "<=",
        PascalTokenType.GreaterEquals => ">=",
        PascalTokenType.GreaterThan => ">",
        PascalTokenType.LeftParen => "(",
        PascalTokenType.RightParen => ")",
        PascalTokenType.LeftBracket => "[",
        PascalTokenType.RightBracket => "]",
        PascalTokenType.LeftBrace => "{",
        PascalTokenType.RightBrace => "}",
        PascalTokenType.UpArrow => "^",
        PascalTokenType.DotDot => "..",
        _ => tokenType.ToString().ToLowerInvariant(),
    };
}
