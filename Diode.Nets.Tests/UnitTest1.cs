using System.Collections.Immutable;

namespace Diode.Nets.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        using (Network.Create(out Subject01 topLevel, Network.Options.LogAll))
        {
            ImmutableList<ElectricalUpdate> log = topLevel.StimulateNets();
            Assert.Collection(log,
                e => Assert.IsType<NetworkStimulus>(e),
                e => Assert.Equal(new NodeUpdate("TopLevel:counter", L6.S, 5), e)
            );
        }

    }
}
