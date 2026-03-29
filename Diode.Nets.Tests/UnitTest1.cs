namespace Diode.Nets.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        using (Network.Create(out Subject01 topLevel))
        {
            topLevel.EnableLogging();
            var log = topLevel.StimulateNets();
            
        }

    }
}
