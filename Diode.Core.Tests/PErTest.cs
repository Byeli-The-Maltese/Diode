using Diode.Core;

namespace Diode.Core.Tests;

public static class PErTest
{
    [Fact]
    public static void Test1()
    {
        Er e1 = new("Test");
        Er e2 = new("Test");
        PEr e3 = new() { RootMessage = "Tests", SubErrors = [e1, e2] };
        Er e4 = new("Abc");
        PEr e5 = new() { RootMessage = "Junk", SubErrors = [e3, e4] };

        string result = """
        Junk
            Tests
                Test
                Test
            Abc
        """;

        Assert.Equal(result, e5.Msg);
    }
}
