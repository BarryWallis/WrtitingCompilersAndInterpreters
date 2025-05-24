using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Intermediate.Implementation;

namespace Intermediate;

/// <summary>
/// Provides factory methods for creating instances of intermediate code objects and nodes.
/// </summary>
/// <remarks>
/// This class is a utility for generating objects that implement the <see cref="IIntermediateCode"/>
/// interface and nodes of type <see cref="IIntermediateCodeNode"/>. It simplifies the creation of intermediate code
/// structures by abstracting the instantiation logic.
/// </remarks>
public class IntermediateCodeFactory
{
    /// <summary>
    /// Creates a new instance of an object that implements the <see cref="IIntermediateCode"/> interface.
    /// </summary>
    /// <returns>An instance of a class that implements <see cref="IIntermediateCode"/>.</returns>
    public static IIntermediateCode CreateIntermediateCode() => new IntermediateCode();

    /// <summary>
    /// Creates a new instance of an intermediate code node with the specified kind.
    /// </summary>
    /// <param name="kind">
    /// The kind of the intermediate code node to create. This parameter determines the type of node being 
    /// instantiated.
    /// </param>
    /// <returns>
    /// An instance of <see cref="IIntermediateCodeNode"/> representing the newly created intermediate code node.
    /// </returns>
    public static IIntermediateCodeNode CreateIntermediateCodeNode(IIntermediateCodeNodeType.Kind kind) 
        => new IntermediateCodeNode(kind);
}
