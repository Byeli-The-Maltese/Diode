using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Diode.Nets;

public partial class Network
{
    public readonly record struct AccessToken : IAccessTokenMint<AccessToken>
    {
        // // // fields
        private static int idCounter = 0;

        internal readonly int idCode;

        // // // constructor

        private AccessToken(int idCode) => this.idCode = idCode;

        // // // methods

        static AccessToken IAccessTokenMint<AccessToken>.MintImpl() => new(Interlocked.Increment(ref idCounter));

        private Rs<Network> TryGetNetwork()
            => networkSingletonStore
            .TryGetValue(this, out Network? accessed)
            ? accessed
            : new Er("Unable to find network");

        public ImmutableList<ElectricalUpdate> Stimulate<T>(Net<T> net, T value, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
        where T : IEquatable<T>
            => TryGetNetwork()
            .Map(nw => nw.Stimulate(net, Voltage<T>.Strong(value), new CallSite(callerFilePath, callerMemberName, callerLineNumber)))
            .OrElse([]);

        public Voltage<T> Probe<T>(Net<T> net)
        where T : IEquatable<T>
            => TryGetNetwork()
            .FlatMap(nw => nw.nodes.TryGet(net))
            .Map(n => n.GetImmediateSample())
            .OrElse(default);

        public void SetOptions(Options options) => TryGetNetwork().Match(nw => nw.SetOptions(options));
    }

    private interface IAccessTokenMint<T>
    where T : IAccessTokenMint<T>
    {
        public static abstract T MintImpl();

        public static T Mint() => T.MintImpl();
    }
}