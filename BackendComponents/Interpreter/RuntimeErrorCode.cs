namespace BackendComponents.Interpreter;

public record RuntimeErrorCode
{
#pragma warning disable IDE1006 // Naming Styles
    public static readonly RuntimeErrorCode UninitializedValue = new("Uninitalized value");
    public static readonly RuntimeErrorCode ValueRange = new("Value out of range");
    public static readonly RuntimeErrorCode InvalidCasae = new("Invalid CASE expression value");
    public static readonly RuntimeErrorCode DivisionByZero = new("Division by zero");
    public static readonly RuntimeErrorCode InvalidStandardFunctionArgument = new("Invalid standard function argument");
    public static readonly RuntimeErrorCode InvalidInput = new("Invalid input");
    public static readonly RuntimeErrorCode StackOverflow = new("Stack overflow");
    public static readonly RuntimeErrorCode UnimplementedFeature = new("Unimplemented runtime feature");
#pragma warning restore IDE1006 // Naming Styles

    public int Status { get; }
    public string Message { get; }

    /// <summary>
    /// For use with the predefined error codes.
    /// </summary>
    /// <param name="message">The message describing the error.</param>
    /// <param name="status">The status code.</param>
    private RuntimeErrorCode(string message, int status = 0)
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
