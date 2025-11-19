using System.Diagnostics;

namespace MessageLib;

/// <summary>
/// Base type for messages dispatched by <see cref="MessageHandler"/>.
/// Concrete messages are implemented as derived records for immutability.
/// </summary>
public record Message
{
    /// <summary>
    /// Throws an exception indicating the current message type was not handled.
    /// Used to fail fast during development when a new message type is introduced.
    /// </summary>
    public void UnknownMessageType() => throw new UnreachableException($"Unhandled message type: {GetType().Name}");
}
