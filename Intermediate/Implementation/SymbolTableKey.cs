namespace Intermediate.Implementation;

public enum SymbolTableKeyType
{
    // Constant.
    ConstantValue,

    // Procedure or function.
    RoutineCode, RoutineSymtab, RoutineICode,
    RoutineParms, RoutineRoutines,

    // Variable or record field value.
    DataValue
}

public class SymbolTableKey : ISymbolTableKey<SymbolTableKeyType>
{
}
