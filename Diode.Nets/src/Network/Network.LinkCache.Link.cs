namespace Diode.Nets;

public partial class Network
{
    partial class LinkCache
    {
        internal abstract class Link(Network network, LinkId id, Sub scope) : CacheItem(network, id, scope)
        {
            public abstract void ReceivePush(VoltagePush push);

            public abstract string GetFullName();

            public abstract string[] GetTargetNodeNames();
        }

        internal abstract class Link<TIn>(Network network, LinkId id, Sub scope, Net<TIn> src) : Link(network, id, scope)
        where TIn : IEquatable<TIn>
        {
            public Net<TIn> Src { get; } = src;

            protected abstract void Input(Voltage<TIn> voltage);

            public sealed override void ReceivePush(VoltagePush push)
            {
                if (push is not VoltagePush<TIn> goodPush)
                    throw new("Wrong push type");
                Input(goodPush.Voltage);
            }

            public override string GetFullName()
                => network
                .nodes
                .TryGet(Src)
                .Map(n => n.GetFullName() + SpiceName.LinkSep + Id.ToString())
                .OrElse(Id.ToString());
        }

        internal sealed class Link<TIn, TOut> : Link<TIn>
        where TIn : IEquatable<TIn>
        where TOut : IEquatable<TOut>
        {
            // // // fields
            private Func<Voltage<TIn>, Voltage<TOut>>? func;
            private bool alreadyDisposed = false;

            // // // constructor

            public Link(Network owner, LinkId id, Sub scope, Net<TIn> src, IEnumerable<Net<TOut>> dsts, Func<Voltage<TIn>, Voltage<TOut>> func) : base(owner, id, scope, src)
            {
                Dsts = dsts.ToHashSet();
                this.func = func;
                network.LogLinkCreation(this);
            }

            // // // properties

            public HashSet<Net<TOut>> Dsts { get; }

            // // // methods

            public override string[] GetTargetNodeNames()
                => Dsts
                .Select(d => network.nodes.TryGet(d))
                .Successes()
                .Select(n => n.GetFullName())
                .ToArray();

            public override void Dispose()
            {
                base.Dispose();
                if (alreadyDisposed) return;
                alreadyDisposed = true;
                network.LogLinkDestruction(this);

                // Tell the source node to stop updating this link
                network!.nodes.TryGet(Src).Match(node => node.StopDrivingLink(Id));
                // Remove this link from the network.
                network!.links.TryRemoveAndDispose(Id);
                // Send a HiZ to the output nodes
                foreach (Net<TOut> dst in Dsts)
                    network!.nodes.TryGet(dst).Match(node => node.TakeInput(Id, default));

                /*
                These fields are the only way a reference to a user type could be leaked.
                Setting them to null is an extra safety measure.
                */
                func = null;
            }

            protected override void Input(Voltage<TIn> sample)
            {
                if (alreadyDisposed) return;

                // Compute the output, even if there are no output nodes.
                network.LogLinkDriveStart(this, sample.Magnitude, sample.RawValue());
                Voltage<TOut> output = func!.Invoke(sample);
                network.LogLinkDriveFinish(this, output.Magnitude, output.RawValue());


                foreach (Net<TOut> dst in Dsts)
                    if (network!.nodes.TryGet(dst).Then(out NodeCache.Node<TOut>? dstNode))
                        dstNode.TakeInput(Id, output);
            }

            protected override Cache<LinkId, Link> GetMyContainer() => network.links;
        }
    }
}