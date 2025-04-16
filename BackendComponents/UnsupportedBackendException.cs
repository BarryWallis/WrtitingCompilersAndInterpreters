
namespace BackendComponents;

/// <summary>
/// Represents an exception for unsupported backend operations. 
/// </summary>
[Serializable]
public class UnsupportedBackendException : Exception
{
    /// <summary>
    /// Exception thrown when an unsupported backend operation is attempted.
    /// </summary>
    /// <param name="operation">Specifies the operation that is not supported by the backend.</param>
    public UnsupportedBackendException(string? operation) : base($"Unsupported Backend: {operation}")
    {
    }

    /// <summary>
    /// Represents an exception for unsupported backend operations.
    /// </summary>
    /// <param name="operation">Specifies the operation that is not supported by the backend.</param>
    /// <param name="innerException">Holds the original exception that caused this exception to be thrown.</param>
    public UnsupportedBackendException(string? operation, Exception? innerException) 
        : base($"Unsupported Backend: {operation}", innerException)
    {
    }
}