using System.Collections.Concurrent;

namespace Diode.Nets;

public partial class Network : IDisposable
{
    // // // fields
    private static readonly ConcurrentDictionary<AccessToken, Network> networkSingletonStore = [];
    public const string TopLevel = nameof(TopLevel);

    private bool alreadyDisposed = false;
    private readonly NodeCache nodes = new();
    private readonly SubCache subs = new();
    private readonly LinkCache links = new();

    private readonly Queue<VoltagePush> pushQueue = new(capacity: 50);
    private readonly List<VoltagePush> executionBuffer = new(capacity: 50);


    // // // constructor

    public Network() => networkSingletonStore[Token] = this;

    // // // properties

    public AccessToken Token { get; } = IAccessTokenMint<AccessToken>.Mint();

    // // // methods

    public void Dispose()
    {
        if (alreadyDisposed) return;
        alreadyDisposed = true;
        GC.SuppressFinalize(this);
        nodes.Dispose();
        subs.Dispose();
        links.Dispose();
        networkSingletonStore.Remove(Token, out _);
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
                links.TryGet(push.PushThroush).Match(link => link.ReceivePush(push));
            executionBuffer.Clear();
        }
    }

    public static Network Create<TCircuit, TPort>(TPort port)
    where TCircuit : ICircuit<TPort>, new()
    where TPort : unmanaged
    {
        var network = new Network();
        network.MintTopLevelCommission(port).Sub<TCircuit, TPort>(port, out _, TopLevel);
        network.FlushVoltages();
        return network;
    }

    public static Network Create<TCircuit>()
    where TCircuit : ICircuit, new()
        => Create<TCircuit, None>(None.Instance);
}