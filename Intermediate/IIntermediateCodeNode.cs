namespace Intermediate;

/// <summary>
/// The interface for the intermediate code node representation.
/// </summary>
public interface IIntermediateCodeNode : ICloneable
{
    IIntermediateCodeNodeType.Kind Kind { get; }

    IIntermediateCodeNode? Parent { get; protected set; }
    IList<IIntermediateCodeNode> Children { get; }

    /// <summary>
    /// Adds a child node to the current intermediate code node.
    /// </summary>
    /// <remarks>This method associates the specified child node with the current node.</remarks>
    /// <param name="child">The child node to add. Will not be added if <see langword="null"/>.</param>
    /// <returns>The child node that was added.</returns>
    IIntermediateCodeNode? AddChild(IIntermediateCodeNode? child);

    /// <summary>
    /// Sets the value of the specified attribute in the intermediate code representation.
    /// </summary>
    /// <param name="key">The key identifying the attribute to set. This cannot be <see langword="null"/>.</param>
    /// <param name="value">The value to assign to the attribute.</param>
    void SetAttribute(IIntermediateCodeKey.Key key, object? value);

    /// <summary>
    /// Retrieves the value of the attribute associated with the specified key.
    /// </summary>
    /// <param name="key">The key identifying the attribute to retrieve. Must not be <see langword="null"/>.</param>
    /// <returns>
    /// The value of the attribute associated with the specified key, or <see langword="null"/> if the key does not 
    /// exist.
    /// </returns>
    object? GetAttribute(IIntermediateCodeKey.Key key);
}
