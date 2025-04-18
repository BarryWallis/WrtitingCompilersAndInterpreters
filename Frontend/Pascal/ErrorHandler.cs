
namespace FrontendComponents.Pascal;

internal class ErrorHandler
{
    public static int ErrorCount { get; internal set; }

    public static void Flag(Token token, PascalErrorCode pascalErrorCode, Parser parser) => throw new NotImplementedException();
    internal static void AbortTranslation(PascalErrorCode iOError, PascalParserTopDown pascalParserTopDown) => throw new NotImplementedException();
}