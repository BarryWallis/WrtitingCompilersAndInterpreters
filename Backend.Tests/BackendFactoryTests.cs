using System;

using Backend.CompilerLib;
using Backend.CompositionLib;
using Backend.InterpreterLib;

using Xunit;

namespace Backend.Tests;

public sealed class BackendFactoryTests
{
    /// <summary>
    /// Verifies CreateBackend returns a <see cref="CodeGenerator"/> for the compile operation.
    /// </summary>
    [Fact]
    public void CreateBackendCompileReturnsCodeGenerator()
    {
        BackendLib.Backend backend = BackendFactory.CreateBackend("compile");

        Assert.IsType<CodeGenerator>(backend);
    }

    /// <summary>
    /// Verifies CreateBackend returns an <see cref="Executor"/> for the execute operation.
    /// </summary>
    [Fact]
    public void CreateBackendExecuteReturnsExecutor()
    {
        BackendLib.Backend backend = BackendFactory.CreateBackend("execute");

        Assert.IsType<Executor>(backend);
    }

    /// <summary>
    /// Verifies CreateBackend throws for unsupported operations.
    /// </summary>
    [Fact]
    public void CreateBackendInvalidOperationThrowsException()
    {
        Exception exception = Assert.Throws<Exception>(() => BackendFactory.CreateBackend("invalid"));

        Assert.Equal("Backend factory: Invalid operation 'invalid'", exception.Message);
    }
}
