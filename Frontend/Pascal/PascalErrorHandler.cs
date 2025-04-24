
using System.Diagnostics;

using Messages;

namespace FrontendComponents.Pascal;

public class PascalErrorHandler
{
    private const int MaxErrors = 10;

    public static int ErrorCount { get; private set; } = 0;

    /// <summary>
    /// Flag an error in the source code. 
    /// </summary>
    /// <param name="token">The bad token.</param>
    /// <param name="errorCode">The Pascal error code.</param>
    /// <param name="parser">The parser information.</param>
    public static void Flag(Token token, PascalErrorCode errorCode, Parser parser)
    {
        Debug.Assert(errorCode.ToString() is not null);

        parser.SendMessage(new SyntaxErrorMessage(token.LineNumber, token.Position, token.Text,
                                                  errorCode.ToString()!));
        if (++ErrorCount > MaxErrors)
        {
            AbortTranslation(PascalErrorCode.TooManyErrors, parser);
        }
    }

    /// <summary>
    /// Abort the translation and return the error code status number to the system.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <param name="parser">The parser information.</param>
    public static void AbortTranslation(PascalErrorCode errorCode, Parser parser)
    {
        string fatalText = $"FATAL ERROR: {errorCode}";
        parser.SendMessage(new SyntaxErrorMessage(0, 0, "", fatalText));
        Environment.Exit(errorCode.Status);
    }
}
