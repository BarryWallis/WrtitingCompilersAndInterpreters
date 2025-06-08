using BackendComponents.Compiler;
using BackendComponents.Interpreter;

namespace BackendComponents;

public static class BackendFactory
{
    public static Backend CreateBackend(string operation)
        => operation.Equals("compile", StringComparison.OrdinalIgnoreCase)
           ? new CodeGenerator()
           : operation.Equals("execute", StringComparison.OrdinalIgnoreCase)
             ? new Executor() as Backend : throw new UnsupportedBackendException(operation);
}
