using System.Collections.Concurrent;

namespace Diode.Nets;

public partial class Network : IDisposable
{
    // // // fields
    private static readonly ConcurrentDictionary<AccessToken, Network> networkSingletonStore = [];
    public const string TopLevel = nameof(TopLevel);

    private bool alreadyDisposed = false;
    private readonly AccessToken token = IAccessTokenMint<AccessToken>.Mint();
    private readonly NodeCache nodes = new();
    private readonly SubCache subs = new();
    private readonly LinkCache links = new();

    private readonly Queue<VoltagePush> pushQueue = new(capacity: 50);
    private readonly List<VoltagePush> executionBuffer = new(capacity: 50);


    // // // constructor

    private Network() => networkSingletonStore[token] = this;

    // // // methods

    public void Dispose()
    {
        if (alreadyDisposed) return;
        alreadyDisposed = true;
        GC.SuppressFinalize(this);
        nodes.Dispose();
        subs.Dispose();
        links.Dispose();
        networkSingletonStore.Remove(token, out _);
    }

    private void Push(VoltagePush push) => pushQueue.Enqueue(push);

    private void FlushVoltages()
    {
        int loops = 0;
        while (pushQueue.Count > 0)
        {
            LogVoltageFlush(loops++);
            while (pushQueue.TryDequeue(out VoltagePush? next))
                executionBuffer.Add(next);
            foreach (VoltagePush push in executionBuffer)
                links.TryGet(push.PushThrough).Match(link => link.ReceivePush(push));
            executionBuffer.Clear();
        }
    }

    public static IDisposable Create<TCircuit>(out TCircuit topLevel, Options options = default)
    where TCircuit : ICircuit<None>, new()
    {
        var network = new Network { options = options };
        network
            .MintTopLevelCommission(None.Instance)
            .Plug(None.Instance)
            .IntoNew<TCircuit>(out Sub topLevelSub, TopLevel)
            ;
        network.FlushVoltages();
        if (network.subs.TryGet(topLevelSub).Unwrap().Circuit is not TCircuit correctTopLevel)
            throw new("The top level circuit is absent, even though it was just created. I give up...");
        topLevel = correctTopLevel;
        return network;
    }
}