using System.Runtime.CompilerServices;

namespace Diode.Nets;

public partial class Network
{

    protected Com<TPort> MintTopLevelCommission<TPort>(TPort port)
    where TPort : unmanaged
        => IComSecret<Com<TPort>, TPort>.Mint<Com<TPort>>(this, port);

    private interface IComSecret<TCom, TPort>
    where TCom : allows ref struct
    where TPort : unmanaged
    {
        public static T Mint<T>(Network authority, TPort port)
        where T : IComSecret<T, TPort>, allows ref struct
            => T.MintSecret(authority, port);

        public static abstract TCom MintSecret(Network authority, TPort port);
    }

    public readonly ref partial struct Com<TPort> : IComSecret<Com<TPort>, TPort>
    where TPort : unmanaged
    {
        // // // fields

        private readonly Network? authority;

        /// <summary>
        /// If this is zero, then created parts will belong to the top-level
        /// namespace, rather than the namespace of a subcircuit
        /// </summary>
        private readonly Sub buildScope;

        private readonly TPort port;

        // // // constructor

        private Com(Network authority, Sub subId, TPort port) => (this.authority, this.buildScope, this.port) = (authority, subId, port);

        static Com<TPort> IComSecret<Com<TPort>, TPort>.MintSecret(Network authority, TPort port) => new(authority, default, port);

        // // // properties
        /// <summary>
        /// This is the port carries information from the hierarchical circuit
        /// parent that might be useful during installation
        /// </summary>
        public TPort Port => port;

        /// <summary>
        /// Gets an access token from the network that issued this commission. Unlike the commission,
        /// the token may be retained by the circuit and used later to interact with the network in
        /// a thread-safe way.
        /// </summary>
        public AccessToken NetworkToken => Authority.token;

        private Network Authority => authority ?? throw new InvalidOperationException("The commission is counterfeit and was never issued by the network");

        private SpiceSpace? NameSpace
            => buildScope == default
            ? null
            : Authority.subs.TryGet(buildScope).Unwrap().NameSpace;

        // // // methods

        public readonly Com<TPort> Net<TNet>(out Net<TNet> net, [CallerArgumentExpression(nameof(net))] string name = "")
        where TNet : IEquatable<TNet>
        {
            SpiceName netName = SpiceName.Create(name).Unwrap(); // This unwrap causes a bonehead exception that should not occur.
            net = Authority.nodes.MakeNew<TNet>(Authority, netName, buildScope);
            return this;
        }

        internal readonly Com<TPort> InternalSub<TCircuit, TCircuitPort>(TCircuitPort port, out Sub sub, string name, bool forExpander, Action<TCircuit>? preInstallationModifier)
        where TCircuit : ICircuit<TCircuitPort>, new()
        where TCircuitPort : unmanaged
        {
            SpiceName netName = SpiceName.Create(name).Unwrap(); // This unwrap causes a bonehead except that should not occur.
            TCircuit circuit = new() { Host = NetworkToken };
            if (preInstallationModifier is not null)
                preInstallationModifier(circuit);
            sub = Authority.subs.MakeNew(Authority, netName, circuit, NameSpace?.Prefix, buildScope, forExpander);
            var innerCommission = new Com<TCircuitPort>(Authority, sub, port);
            circuit.Install(innerCommission);
            return this;
        }

        public readonly Com<TPort> GetBuildScope(out Sub yourself)
        {
            yourself = buildScope;
            return this;
        }

        public readonly Com<TPort> GetPort(out TPort port)
        {
            port = Port;
            return this;
        }
    }


}