using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommonInterfaces;

namespace FrontendComponents.Pascal;
public class PascalTokenType : ITokenType
{
    const int _firstReservedIndex = (int)ITokenType.Kind.And;
    const int _lastReservedIndex = (int)ITokenType.Kind.With;
    const int _firstSpecialSymbolIndex = (int)ITokenType.Kind.Plus;
    const int _lastSpecialSymbolIndex = (int)ITokenType.Kind.DotDot;

    public static readonly HashSet<string> ReservedWords =
    [
        "and", "array", "begin", "case", "const", "div", "do", "downto", "else",
        "end", "file", "for", "function", "goto", "if", "in", "label", "mod",
        "nil", "not", "of", "or", "packed", "procedure", "program", "record",
        "repeat", "set", "then", "to", "type", "until", "var", "while", "with"
    ];

    private static readonly string[] _specialSymbols =
    [
        "+", "-", "*", "/", ":=", ".", ",", ";", ":", "'", "=", "<>", "<", "<=", ">=", ">", "(", ")", "[",
        "]", "{", "}", "^", ".."
    ];
    public static readonly Dictionary<string, ITokenType.Kind> SpecialSymbols = [];

    // Initialize the special symbols dictionary with Pascal's special symbols
    static PascalTokenType()
        => SpecialSymbols = Enumerable.Range(0, _lastSpecialSymbolIndex - _firstSpecialSymbolIndex + 1)
                                      .ToDictionary
                                       (
                                            static i => _specialSymbols[i],
                                            static i => (ITokenType.Kind)(i + _firstSpecialSymbolIndex)
                                        );
}
