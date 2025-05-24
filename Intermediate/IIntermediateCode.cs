namespace Intermediate;

/// <summary>
/// The interface for the intermediate code representation.
/// </summary>
public interface IIntermediateCode
{
    IIntermediateCodeNode? Root { get; set; }
}
