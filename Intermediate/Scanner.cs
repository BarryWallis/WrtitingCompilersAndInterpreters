using FrontendIntermediateMutual;

namespace Intermediate;

public abstract class Scanner
{
    public Token CurrentToken { get; }
    public Token GetNextToken { get; }
}