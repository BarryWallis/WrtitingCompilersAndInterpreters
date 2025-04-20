
using System.Diagnostics;

using Messages;

namespace FrontendComponents.Pascal;

public class ErrorHandler
{
    private const int _maxErrors = 25;

    public static int ErrorCount {get; set;} = 0;

    public static void Flag(Token token, PascalErrorCode errorCode, Parser parser)
    {
        Debug.Assert(errorCode.ToString() is not null);

        parser.SendMessage(new SyntaxErrorMessage(token.LineNumber, token.Position, token.Text, errorCode.ToString()!));
        if (++ErrorCount > _maxErrors)
        {
            AbortTranslation(PascalErrorCode.TooManyErrors, parser);
        }
    }

    public static void AbortTranslation(PascalErrorCode errorCode, Parser parser)
    {
        string fatalText = $"FATAL ERROR: {errorCode}";
        parser.SendMessage(new SyntaxErrorMessage(0, 0, null, fatalText));
        Environment.Exit(errorCode.Status);
    }
}