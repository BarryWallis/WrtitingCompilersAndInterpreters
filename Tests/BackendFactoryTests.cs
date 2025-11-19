using BackendLib;

using Xunit;

namespace Tests;

public class BackendFactoryTests
{
    [Fact]
    public void CreateBackend_Compile()
    {
        Backend backend = BackendFactory.CreateBackend("compile");
        Assert.NotNull(backend);
    }

    [Fact]
    public void CreateBackend_Execute()
    {
        Backend backend = BackendFactory.CreateBackend("execute");
        Assert.NotNull(backend);
    }

    [Fact]
    public void CreateBackend_Invalid_ShouldThrow() => Assert.Throws<ArgumentException>(() => BackendFactory.CreateBackend("other"));
}
