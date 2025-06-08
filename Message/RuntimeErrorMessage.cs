namespace Messages;

public record RuntimeErrorMessage(string ErrorMessage, int? LineNumber) : Message
{

}
