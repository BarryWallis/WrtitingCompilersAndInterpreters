using System.Diagnostics;

namespace Intermediate.Implementation;

/// <summary>
/// An implementation of the <see cref="IIntermediateCodeNode"/> interface, representing a node in the intermediate code tree.
/// </summary>
/// <param name="kind">The node kind whose name will be the name of this node.</param>
public class IntermediateCodeNode(IIntermediateCodeNodeType.Kind kind) : IIntermediateCodeNode
{
    private readonly Dictionary<IIntermediateCodeKey.Key, object?> _attributes = [];

    public IList<KeyValuePair<IIntermediateCodeKey.Key, object?>> Attributes => [.. _attributes];
    public IIntermediateCodeNodeType.Kind Kind { get; } = kind;
    public IIntermediateCodeNode? Parent { get; set; } = null;
    public IList<IIntermediateCodeNode> Children { get; } = [];

    /// <summary>
    /// Add a child node to the current node.
    /// </summary>
    /// <param name="node">The child node to be added. If the child node is <see langword="null"/>, it is not added.</param>
    /// <returns>The added child node, or <see langword="null"/> if the node was not added.</returns>
    public IIntermediateCodeNode? AddChild(IIntermediateCodeNode? node)
    {
        if (node is not null)
        {
            Children.Add(node);
            Debug.Assert(node is IntermediateCodeNode);
            (node as IntermediateCodeNode)!.Parent = this;
        }

        return node;
    }

    /// <summary>
    /// Creates a deep copy of the current node.
    /// </summary>
    /// <returns>A new instance of <see cref="IIntermediateCodeNode"/> that is a copy of the current node.</returns>
    public IIntermediateCodeNode Clone()
    {
        IntermediateCodeNode? copy = IntermediateCodeFactory.CreateIntermediateCodeNode(Kind) as IntermediateCodeNode;
        Debug.Assert(copy is not null);

        foreach (KeyValuePair<IIntermediateCodeKey.Key, object?> attribute in _attributes)
        {
            copy.SetAttribute(attribute.Key, attribute.Value);
        }

        return copy;
    }

    /// <summary>
    /// Get the value of the given attribute.
    /// </summary>
    /// <param name="key">The attribute key.</param>
    /// <returns>The attribute value or <see langword="null"/> if the attribute doesn't exist.</returns>
    public object? GetAttribute(IIntermediateCodeKey.Key key)
        => _attributes.TryGetValue(key, out object? value) ? value : null;

    /// <summary>
    /// Set a node attribute.
    /// </summary>
    /// <param name="key">The attribute key.</param>
    /// <param name="value">The attribute value.</param>
    public void SetAttribute(IIntermediateCodeKey.Key key, object? value) => _attributes.Add(key, value);

    /// <summary>
    /// Returns a string representation of the current instance.
    /// </summary>
    /// <returns>A string that represents the value of the <see cref="Kind"/> property.</returns>
    public override string? ToString() => Kind.ToString();

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <remarks>This method is an explicit implementation of the <see cref="ICloneable.Clone"/> method  and
    /// internally calls the <c>Clone</c> method of the current instance.</remarks>
    /// <returns>A new object that is a copy of this instance.</returns>
    object ICloneable.Clone() => Clone();
}

