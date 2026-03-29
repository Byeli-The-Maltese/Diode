using System.Collections.Immutable;

namespace Diode.Nets.Tests;

public class Subject01 : ICircuit
{
    public Net<int> counter;

    public Network.AccessToken Host { get; init; }

    public Network.Com<None> Install(Network.Com<None> com)
        => com
        .Net(out counter)
        ;

    public ImmutableList<ElectricalUpdate> StimulateNets() => Host.Stimulate(counter, 5);

    internal void EnableLogging() => Host.SetOptions(Network.Options.LogAll);
}
