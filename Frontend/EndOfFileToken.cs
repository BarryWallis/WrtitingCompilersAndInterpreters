namespace FrontendLib;

/// <summary>
/// Token representing the end-of-file sentinel from the <see cref="Source"/>.
/// </summary>
public class EndOfFileToken(Source source) : Token(source)
{
    /// <summary>
    /// EOF token does not extract any additional characters.
    /// </summary>
    protected override void Extract()
    {
    }
}
