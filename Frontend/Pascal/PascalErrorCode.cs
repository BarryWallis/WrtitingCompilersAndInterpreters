namespace FrontendComponents.Pascal;

/// <summary>
/// Pascal translation error codes.
/// </summary>
public record PascalErrorCode
{
#pragma warning disable IDE1006 // Naming Styles
    public static readonly PascalErrorCode AlreadyForwarded = new("Already specified in FORWARD");
    public static readonly PascalErrorCode IdentifierRedefined = new("Redefined identifier");
    public static readonly PascalErrorCode IdentifierUndefined = new("Undefined identifier");
    public static readonly PascalErrorCode IncompatibleAssignment = new("Incompatible assignment");
    public static readonly PascalErrorCode IncompatibleTypes = new("Incompatible types");
    public static readonly PascalErrorCode InvalidAssignment = new("Invalid assignment statement");
    public static readonly PascalErrorCode InvalidCharacter = new("Invalid character");
    public static readonly PascalErrorCode InvalidConstant = new("Invalid constant");
    public static readonly PascalErrorCode InvalidExponent = new("Invalid exponent");
    public static readonly PascalErrorCode InvalidExpression = new("Invalid expression");
    public static readonly PascalErrorCode InvalidField = new("Invalid field");
    public static readonly PascalErrorCode InvalidFraction = new("Invalid fraction");
    public static readonly PascalErrorCode InvalidIdentifierUsage = new("Invalid identifier usage");
    public static readonly PascalErrorCode InvalidIndexType = new("Invalid index type");
    public static readonly PascalErrorCode InvalidNumber = new("Invalid number");
    public static readonly PascalErrorCode InvalidStatement = new("Invalid statement");
    public static readonly PascalErrorCode InvalidSubrangeType = new("Invalid subrange type");
    public static readonly PascalErrorCode InvalidTarget = new("Invalid assignment target");
    public static readonly PascalErrorCode InvalidType = new("Invalid type");
    public static readonly PascalErrorCode InvalidVarParm = new("Invalid Var parameter");
    public static readonly PascalErrorCode MinGtMax = new("Min limit greater than max limit");
    public static readonly PascalErrorCode MissingBegin = new("Missing Begin");
    public static readonly PascalErrorCode MissingColon = new("Missing :");
    public static readonly PascalErrorCode MissingColonEquals = new("Missing :=");
    public static readonly PascalErrorCode MissingComma = new("Missing ,");
    public static readonly PascalErrorCode MissingConstant = new("Missing constant");
    public static readonly PascalErrorCode MissingDo = new("Missing Do");
    public static readonly PascalErrorCode MissingDotDot = new("Missing ..");
    public static readonly PascalErrorCode MissingEnd = new("Missing End");
    public static readonly PascalErrorCode MissingEquals = new("Missing =");
    public static readonly PascalErrorCode MissingForControl = new("Invalid For control variable");
    public static readonly PascalErrorCode MissingIdentifier = new("Missing identifier");
    public static readonly PascalErrorCode MissingLeftBracket = new("Missing [");
    public static readonly PascalErrorCode MissingOf = new("Missing Of");
    public static readonly PascalErrorCode MissingPeriod = new("Missing .");
    public static readonly PascalErrorCode MissingProgram = new("Missing Program");
    public static readonly PascalErrorCode MissingRightBracket = new("Missing ]");
    public static readonly PascalErrorCode MissingRightParen = new("Missing )");
    public static readonly PascalErrorCode MissingSemicolon = new("Missing ;");
    public static readonly PascalErrorCode MissingThen = new("Missing Then");
    public static readonly PascalErrorCode MissingToDownto = new("Missing To or Downto");
    public static readonly PascalErrorCode MissingUntil = new("Missing Until");
    public static readonly PascalErrorCode MissingVariable = new("Missing variable");
    public static readonly PascalErrorCode CaseConstantReused = new("Case constant reused");
    public static readonly PascalErrorCode NotConstantIdentifier = new("Not a constant identifier");
    public static readonly PascalErrorCode NotRecordVariable = new("Not a record variable");
    public static readonly PascalErrorCode NotTypeIdentifier = new("Not a type identifier");
    public static readonly PascalErrorCode RangeInteger = new("Integer literal out of range");
    public static readonly PascalErrorCode RangeReal = new("Real literal out of range");
    public static readonly PascalErrorCode StackOverflow = new("Stack overflow");
    public static readonly PascalErrorCode TooManyLevels = new("Nesting level too deep");
    public static readonly PascalErrorCode TooManySubscripts = new("Too many subscripts");
    public static readonly PascalErrorCode UnexpectedEof = new("Unexpected end of file");
    public static readonly PascalErrorCode UnexpectedToken = new("Unexpected token");
    public static readonly PascalErrorCode Unimplemented = new("Unimplemented feature");
    public static readonly PascalErrorCode Unrecognizable = new("Unrecognizable input");
    public static readonly PascalErrorCode WrongNumberOfParms
                                            = new("Wrong number of actual parameters");

    public static readonly PascalErrorCode IOError = new("Object I/O error", -101);
    public static readonly PascalErrorCode TooManyErrors = new("Too many syntax errors", -102);
#pragma warning restore IDE1006 // Naming Styles

    public int Status { get; }
    public string Message { get; }

    /// <summary>
    /// For use with the predefined error codes.
    /// </summary>
    /// <param name="message">The message describing the error.</param>
    /// <param name="status">The status code.</param>
    private PascalErrorCode(string message, int status = 0)
    {
        Status = status;
        Message = message;
    }

    /// <summary>
    /// Return the error message.
    /// </summary>
    /// <returns>The error message.</returns>
    public override string? ToString() => Message;
}
