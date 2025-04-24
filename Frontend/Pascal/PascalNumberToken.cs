using System.Text;

using CommonInterfaces;

namespace FrontendComponents.Pascal;

/// <summary>
/// A class representing a number token in Pascal.
/// </summary>
public record PascalNumberToken : PascalToken
{
    private const int MaxExponent = 1020;

    /// <summary>
    /// Create a new NumberToken using the provided source. The token type is determined by the source.
    /// </summary>
    /// <param name="source">The source program from which the token is created.</param>
    public PascalNumberToken(Source source) : base(source)
    {
    }

    /// <summary>
    /// Extract a NumberToken from the source program. The token type is determined by the source data.
    /// </summary>
    protected override void Extract()
    {
        StringBuilder textBuffer = new();
        ExtractNumber(textBuffer);
        Text = textBuffer.ToString();
    }

    /// <summary>
    /// Extract a number from the source program. The token type is determined by the source data.
    /// </summary>
    /// <param name="textBuffer">The source to extract the number from.</param>
    private void ExtractNumber(StringBuilder textBuffer)
    {
        string? wholeDigits;
        string? fractionDigits = null;
        string? exponentDigits = null;
        char exponentSign = '+';
        bool sawDotDot = false;
        char currentChar;

        Kind = ITokenType.Kind.Integer;
        wholeDigits = UnsignedIntegerDigits(textBuffer);

        if (Kind == ITokenType.Kind.Error)
        {
            return;
        }

        currentChar = CurrentChar();
        if (currentChar == '.')
        {
            bool flowControl = ExtractFraction(textBuffer, ref fractionDigits, ref sawDotDot, currentChar);
            if (!flowControl)
            {
                return;
            }
        }

        currentChar = CurrentChar();
        if (!sawDotDot && ((currentChar == 'E') || (currentChar == 'e')))
        {
            exponentDigits = ExtractExponent(textBuffer, ref exponentSign, ref currentChar);
        }

        if (Kind == ITokenType.Kind.Integer)
        {
            int integerValue = ComputeIntegerValue(wholeDigits);
            if (Kind != ITokenType.Kind.Error)
            {
                Value = integerValue;
            }
        }
        else if (Kind == ITokenType.Kind.Real)
        {
            float floatValue = ComputeFloatValue(wholeDigits, fractionDigits, exponentDigits,
                                                 exponentSign);
            if (Kind != ITokenType.Kind.Error)
            {
                Value = floatValue;
            }
        }
    }

    /// <summary>
    /// Extract the exponent part of a number from the source program.
    /// </summary>
    /// <param name="textBuffer">The source to extract the exponent from.</param>
    /// <param name="exponentSign">The sign of the exponent ('+' or '-').</param>
    /// <param name="currentChar">The current character being processed.</param>
    /// <returns>A string containing the exponent digits.</returns>
    private string? ExtractExponent(StringBuilder textBuffer, ref char exponentSign, ref char currentChar)
    {
        string? exponentDigits;
        Kind = ITokenType.Kind.Real;
        _ = textBuffer.Append(currentChar);
        currentChar = GetNextChar();

        if (currentChar is '+' or '-')
        {
            _ = textBuffer.Append(currentChar);
            exponentSign = currentChar;
            _ = GetNextChar();
        }

        exponentDigits = UnsignedIntegerDigits(textBuffer);
        return exponentDigits;
    }

    /// <summary>
    /// Extract the fraction part of a number from the source program.
    /// </summary>
    /// <param name="textBuffer">The source to extract the fraction from.</param>
    /// <param name="fractionDigits">The extracted fraction digits.</param>
    /// <param name="sawDotDot">Indicates if a '..' was encountered.</param>
    /// <param name="currentChar">The current character being processed.</param>
    /// <returns>True if the extraction was successful; otherwise, false.</returns>
    private bool ExtractFraction(StringBuilder textBuffer, ref string? fractionDigits, ref bool sawDotDot, char currentChar)
    {
        if (PeekChar() == '.')
        {
            sawDotDot = true;
        }
        else
        {
            Kind = ITokenType.Kind.Real;
            _ = textBuffer.Append(currentChar);
            _ = GetNextChar();
            fractionDigits = UnsignedIntegerDigits(textBuffer);
            if (Kind == ITokenType.Kind.Error)
            {
                return false;
            }
        }

        return true;
    }
    /// <summary>
    /// Compute the floating point value of a number based on its components.
    /// </summary>
    /// <param name="wholeDigits">The whole number part of the number.</param>
    /// <param name="fractionDigits">The fractional part of the number.</param>
    /// <param name="exponentDigits">The exponent part of the number.</param>
    /// <param name="exponentSign">The sign of the exponent.</param>
    /// <returns>The computed floating point value.</returns>
    private float ComputeFloatValue(string? wholeDigits, string? fractionDigits, string? exponentDigits,
                                    char exponentSign)
    {
        double floatValue = 0.0;
        int exponentValue = ComputeIntegerValue(exponentDigits);
        string digits = wholeDigits ?? string.Empty;
        if (exponentSign == '-')
        {
            exponentValue = -exponentValue;
        }

        if (fractionDigits is not null)
        {
            exponentValue -= fractionDigits.Length;
            digits += fractionDigits;
        }

        if (Math.Abs(exponentValue + (wholeDigits?.Length ?? 0)) > MaxExponent)
        {
            Kind = ITokenType.Kind.Error;
            Value = PascalErrorCode.RangeReal;
            return 0.0f;
        }

        int index = 0;
        while (index < digits.Length)
        {
            floatValue = (10.0f * floatValue) + char.GetNumericValue(digits, index++);
        }

        if (exponentValue != 0)
        {
            floatValue *= Math.Pow(10.0, exponentValue);
        }

        return (float)floatValue;
    }

    /// <summary>
    /// Compute the integer value of a number based on its digits.
    /// </summary>
    /// <param name="digits">The digits of the number.</param>
    /// <returns>The computed integer value.</returns>
    private int ComputeIntegerValue(string? digits)
    {
        if (digits is null)
        {
            return 0;
        }

        int integerValue = 0;
        int previousValue = -1;
        int index = 0;
        while (index < digits.Length && integerValue >= previousValue)
        {
            previousValue = integerValue;
            integerValue = (10 * integerValue) + (int)char.GetNumericValue(digits, index++);
        }

        if (integerValue >= previousValue)
        {
            return integerValue;
        }
        else
        {
            Kind = ITokenType.Kind.Error;
            Value = PascalErrorCode.RangeInteger;
            return 0;
        }
    }

    /// <summary>
    /// Extract the unsigned integer digits from the source program.
    /// </summary>
    /// <param name="textBuffer">The buffer to extract digits from.</param>
    /// <returns>The extracted digits as a string.</returns>
    private string? UnsignedIntegerDigits(StringBuilder textBuffer)
    {
        char currentChar = CurrentChar();
        if (!char.IsDigit(currentChar))
        {
            Kind = ITokenType.Kind.Error;
            Value = PascalErrorCode.InvalidNumber;
            return null;
        }

        StringBuilder digits = new();
        while (char.IsDigit(currentChar))
        {
            _ = textBuffer.Append(currentChar);
            _ = digits.Append(currentChar);
            currentChar = GetNextChar();
        }

        return digits.ToString();
    }
}
