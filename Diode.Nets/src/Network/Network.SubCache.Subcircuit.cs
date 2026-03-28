namespace Diode.Nets;

public partial class Network
{
    partial class SubCache
    {
        internal sealed class Subcircuit : CacheItem
        {
            // // // fields
            private bool alreadyDisposed = false;

            // // // constructor

            public Subcircuit(Network network, Sub id, SpiceName name, SpiceSpace nameSpace, object circuit, Sub scope) : base(network, id, scope)
            {
                Name = name;
                NameSpace = nameSpace;
                Circuit = circuit;
                this.network.LogSubCreation(this);
            }

            // // // properties

            public SpiceName Name { get; }

            public SpiceSpace NameSpace { get; }

            public object Circuit { get; }

            // // // methods

            public string GetFullName() => NameSpace.Prefix;

            protected override SubCache GetMyContainer() => network.subs;

            public override void Dispose()
            {
                base.Dispose();
                if (alreadyDisposed) return;
                alreadyDisposed = true;
                network.LogSubDestruction(this);

                (Circuit as IDisposable)?.Dispose();
                foreach (IDisposable node in NameSpace.Nets.Values.Select(network.nodes.TryGet).Successes().ToArray())
                    node.Dispose();
                foreach (IDisposable sub in NameSpace.Subs.Values.Select(network.subs.TryGet).Successes().ToArray())
                    sub.Dispose();
                foreach (IDisposable link in NameSpace.Links.Values.Select(network.links.TryGet).Successes().ToArray())
                    link.Dispose();
            }
        }
    }

}