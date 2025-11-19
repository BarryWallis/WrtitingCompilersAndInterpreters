namespace BackendLib;

public class BackendFactory
{
    public static Backend CreateBackend(string operation)
        => operation.Equals("compile", StringComparison.OrdinalIgnoreCase)
            ? new Compiler.CodeGenerator()
            : operation.Equals("execute", StringComparison.OrdinalIgnoreCase)
                ? (Backend)new Interpreter.Executor()
                : throw new ArgumentException($"Unsupported backend type: {operation}");
}
