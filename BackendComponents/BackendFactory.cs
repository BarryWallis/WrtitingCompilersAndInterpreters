using BackendComponents.Compiler;
using BackendComponents.Interpreter;

namespace BackendComponents;

public static class BackendFactory
{
    public static Backend CreateBackend(string operation)
        => operation.Equals("compile", StringComparison.OrdinalIgnoreCase)
           ? new CodeGenerator()
           : operation.Equals("execute", StringComparison.OrdinalIgnoreCase)
             ? (Backend)new Executor()
             : throw new UnsupportedBackendException(operation);
}
