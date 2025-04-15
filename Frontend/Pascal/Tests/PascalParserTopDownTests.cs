using System;
using System.Diagnostics;
using System.IO;
using Moq;
using Xunit;
using FrontendComponents;
using FrontendComponents.Pascal;

namespace FrontendComponents.Pascal.Tests;

public class PascalParserTopDownTests
{
    [Fact]
    public void ErrorCount_DefaultValue_IsZero()
    {
        // Arrange
        Mock<Scanner> mockScanner = new(new Source(new StreamReader(new MemoryStream())));
        PascalParserTopDown parser = new(mockScanner.Object);

        // Act
        int errorCount = parser.ErrorCount;

        // Assert
        Assert.Equal(0, errorCount);
    }
}