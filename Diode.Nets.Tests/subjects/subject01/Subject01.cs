using System.Collections.Immutable;

namespace Diode.Nets.Tests;

public class Subject01 : ICircuit
{
    public Net<int> counter;
    private Network.AccessToken host;

    public Network.Com<None> Install(Network.Com<None> com)
        => com
        .GetAccessToken(out host)
        .Net(out counter)
        ;

    public ImmutableList<ElectricalUpdate> StimulateNets() => host.Stimulate(counter, 5);
}
