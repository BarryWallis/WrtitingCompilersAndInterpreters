using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommonInterfaces;

namespace FrontendComponents.Pascal;
public class PascalTokenType : ITokenType
{
    private const int FirstSpecialSymbolIndex = (int)ITokenType.Kind.Plus;
    private const int LastSpecialSymbolIndex = (int)ITokenType.Kind.DotDot;

    public static readonly HashSet<string> _reservedWords =
    [
        "and", "array", "begin", "case", "const", "div", "do", "downto", "else",
        "end", "file", "for", "function", "goto", "if", "in", "label", "mod",
        "nil", "not", "of", "or", "packed", "procedure", "program", "record",
        "repeat", "set", "then", "to", "type", "until", "var", "while", "with"
    ];

    private static readonly string[] _specialSymbolLiterals =
    [
        "+", "-", "*", "/", ":=", ".", ",", ((char)59).ToString(), ":", "'", "=", "<>", "<", "<=", ">=", ">", "(", ")", "[",
        "]", "{", "}", "^", ".."];

#pragma warning disable IDE1006 // Naming Styles
    public static readonly Dictionary<string, ITokenType.Kind> SpecialSymbols = [];
#pragma warning restore IDE1006 // Naming Styles

    // Initialize the special symbols dictionary with Pascal's special symbols
    static PascalTokenType()
        => SpecialSymbols = Enumerable.Range(0, LastSpecialSymbolIndex - FirstSpecialSymbolIndex + 1)
                                      .ToDictionary
                                       (
                                            static i => _specialSymbolLiterals[i],
                                            static i => (ITokenType.Kind)(i + FirstSpecialSymbolIndex)
                                        );
}
