using FrontendLib;

namespace Frontend.PascalLib;

public class PascalErrorHandler
{
    public int ErrorCount { get; internal set; }

    public void Flag(Token token, PascalErrorCode pascalErrorCode, PascalParserTopDown pascalParserTopDown) => throw new NotImplementedException();
    internal void AbortTranslation(PascalErrorCode iO_ERROR, PascalParserTopDown pascalParserTopDown) => throw new NotImplementedException();
}
