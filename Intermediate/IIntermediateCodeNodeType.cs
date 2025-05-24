namespace Intermediate;

/// <summary>
/// The IIntermediateCodeNodeType interface defines the types of nodes in the intermediate code representation.
/// </summary>
public interface IIntermediateCodeNodeType
{
    /// <summary>
    /// The Kind enumeration defines the various kinds of nodes that can be present in the intermediate code.
    /// </summary>
    enum Kind
    {
        // Program Structure
        Program, Procedure, Function,

        // Statements
        Compound, Assign, Loop, Test, Call, Parameters,
        If, Select, Select_Branch, Select_Constants, No_Op,

        // Relational Operators
        Eq, Ne, Lt, Le, Gt, Ge, Not,

        // Additive Operators
        Add, Subtract, Or, Negate,

        // Multiplicative Operators
        Multiply, Integer_Divide, Float_Divide, Mod, And,

        // Operands
        Variable, Subscripts, Field,
        Integer_Constant, Real_Constant,
        String_Constant, Boolean_Constant,
    }
}
