using System.Diagnostics;

using Intermediate;

namespace Frontend;

/// <summary>
/// Abstract class for parsing, utilizing a scanner to read tokens. It maintains an error count and provides 
/// methods to parse and retrieve tokens. It will be implemented by language specific subclasses.
/// </summary>
/// <remarks>
/// Initializes a new instance of the Parser class with a specified scanner for processing input.
/// </remarks>
/// <param name="scanner">The input processing tool used to analyze and interpret data.</param>
public abstract class Parser(Scanner scanner)
{
    protected static ISymbolTable? SymbolTable { get; set; } = null;

    protected Scanner Scanner { get; init; } = scanner;

    protected IntermediateCode? IntermediateCode { get; set; } = null;

    /// <summary>
    /// Represents the number of errors encountered.
    /// </summary>
    public abstract int ErrorCount { get; protected set; }

    /// <summary>
    /// Gets the current currentToken from the scanner. 
    /// </summary>
    public Token CurrentToken
    {
        get
        {
            Token? currentToken = Scanner.CurrentToken;

            Debug.Assert(currentToken is not null);
            return currentToken;
        }
    }

    /// <summary>
    /// Parses data from a source. 
    /// </summary>
    public abstract void Parse();

    /// <summary>
    /// Retrieves the next currentToken from the scanner.
    /// </summary>
    /// <returns>Returns the next currentToken available from the scanning process.</returns>
    public Token GetNextToken()
    {
        Token token = Scanner.GetNextToken();

        Debug.Assert(token is not null);
        return token;
    }
}