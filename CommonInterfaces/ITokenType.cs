namespace CommonInterfaces;

/// <summary>
/// The interface for token types.
/// </summary>
public interface ITokenType
{
    enum Kind
    {
        Placeholder, // TODO: Remove when all tokens are implemented

        // Reserved Words
        And, Array, Begin, Case, Const, Div, Do, Downto, Else, End, File, For, Function, Goto, If, In,
        Label, Mod, Nil, Not, Of, Or, Packed, Procedure, Program, Record, Repeat, Set, Then, To, Type,
        Until, Var, While, With,

        // Special Symbols
        Plus, Minus, Star, Slash, ColonEquals, Dot, Comma, Semicolon, Colon, Quote, Equals,
        NotEquals, LessThan, LessEquals, GreaterEquals, GreaterThan, LeftParen, RightParen,
        LeftBracket, RightBracket, LeftBrace, RightBrace, UpArrow, DotDot,

        Identifier, Integer, Real, String, Error, EndOfFile
    }
}
