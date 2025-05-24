
using System.Diagnostics;

using Messages;

namespace FrontendComponents.Pascal;

/// <summary>
/// Handles errors in the Pascal frontend.
/// </summary>
public class ErrorHandler
{
    private const int MaxErrors = 25;

    public static int ErrorCount { get; set; } = 0;

    /// <summary>
    /// Flags a syntax error in the source code and sends an error message to the parser.
    /// </summary>
    /// <remarks>
    /// If the total number of errors exceeds the maximum allowed, the translation process is
    /// aborted.
    /// </remarks>
    /// <param name="token">The token where the error occurred, including its line number, position, and text.</param>
    /// <param name="errorCode">The specific error code representing the type of syntax error.</param>
    /// <param name="parser">The parser instance responsible for handling the error message.</param>
    public static void Flag(Token token, PascalErrorCode errorCode, Parser parser)
    {
        Debug.Assert(errorCode.ToString() is not null);

        parser.SendMessage(new SyntaxErrorMessage(token.LineNumber, token.Position, token.Text, errorCode.ToString()!));
        if (++ErrorCount > MaxErrors)
        {
            AbortTranslation(PascalErrorCode.TooManyErrors, parser);
        }
    }

/// <summary>
/// Aborts the translation process and terminates the application with the specified error code.
/// </summary>
/// <remarks>
/// This method sends a fatal error message using the provided <paramref name="parser"/>  and then
/// terminates the application by calling <see cref="Environment.Exit(int)"/>  with the status code from <paramref name="errorCode"/>.
/// </remarks>
/// <param name="errorCode">
/// The <see cref="PascalErrorCode"/> representing the error that caused the translation to abort.  This determines the
/// exit status of the application.
/// </param>
/// <param name="parser">The <see cref="Parser"/> instance used to send the error message before termination.</param>
    public static void AbortTranslation(PascalErrorCode errorCode, Parser parser)
    {
        string fatalText = $"FATAL ERROR: {errorCode}";
        parser.SendMessage(new SyntaxErrorMessage(0, 0, null, fatalText));
        Environment.Exit(errorCode.Status);
    }
}
