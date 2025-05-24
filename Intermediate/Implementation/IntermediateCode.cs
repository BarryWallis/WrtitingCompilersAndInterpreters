namespace Intermediate.Implementation;

/// <summary>
/// The implementation of the intermediate code representation.
/// </summary>
public class IntermediateCode : IIntermediateCode
{
    public IIntermediateCodeNode? Root { get; set; } = null;
}
