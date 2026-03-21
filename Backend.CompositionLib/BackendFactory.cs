using System;

using Backend.CompilerLib;
using Backend.InterpreterLib;

namespace Backend.CompositionLib;

/// <summary>
/// Composes backend abstractions with concrete compiler and interpreter implementations.
/// </summary>
public static class BackendFactory
{
    /// <summary>
    /// Creates a compiler or interpreter backend component for the requested operation.
    /// </summary>
    /// <param name="operation">The backend operation, either "compile" or "execute".</param>
    /// <returns>A backend component matching the requested operation.</returns>
    /// <exception cref="Exception">Thrown when <paramref name="operation"/> is not a supported backend operation.</exception>
    public static BackendLib.Backend CreateBackend(string operation)
        => string.Equals(operation, "compile", StringComparison.OrdinalIgnoreCase)
           ? new CodeGenerator()
           : string.Equals(operation, "execute", StringComparison.OrdinalIgnoreCase)
              ? (BackendLib.Backend)new Executor()
              : throw new Exception($"Backend factory: Invalid operation '{operation}'");
}
