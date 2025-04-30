using CommonInterfaces;

namespace FrontendComponents.Pascal;

/// <summary>
/// Pascal error token.
/// </summary>
public record PascalErrorToken : PascalToken
{
    /// <summary>
    /// Create a new error token. 
    /// </summary>
    /// <param name="source">The source from where to fetch subsequent characters.</param>
    /// <param name="errorCode">The error code.</param>
    /// <param name="text">The text of the erroneous token.</param>
    public PascalErrorToken(Source source, PascalErrorCode errorCode, string text) : base(source)
    {
        Kind = ITokenType.Kind.Error;
        Value = errorCode;
        Text = text;
    }

    /// <summary>
    /// Do nothing.  Do not consume any source characters.
    /// </summary>
    /// <exception cref="NotImplementedException">
    /// <see cref="Extract"/> not implemented for <see cref="PascalErrorToken"/>.
    /// </exception>
    protected override void Extract()
    {
        // Do nothing.  Do not consume any source characters.
    }
}
