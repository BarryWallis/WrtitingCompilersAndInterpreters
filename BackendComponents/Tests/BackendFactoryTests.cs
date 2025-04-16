using System;
using Xunit;
using BackendComponents;
using BackendComponents.Compiler;
using BackendComponents.Interpreter;

namespace BackendComponents.Tests;

public class BackendFactoryTests
{
    [Fact]
    public void CreateBackend_CompileOperation_ReturnsCodeGenerator()
    {
        // Act
        Backend backend = BackendFactory.CreateBackend("compile");

        // Assert
        _ = Assert.IsType<CodeGenerator>(backend);
    }

    [Fact]
    public void CreateBackend_ExecuteOperation_ReturnsExecutor()
    {
        // Act
        Backend backend = BackendFactory.CreateBackend("execute");

        // Assert
        _ = Assert.IsType<Executor>(backend);
    }

    [Fact]
    public void CreateBackend_InvalidOperation_ThrowsUnsupportedBackendException() 
        => _ = Assert.Throws<UnsupportedBackendException>(() 
                      => BackendFactory.CreateBackend("invalid"));
}