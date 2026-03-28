namespace Diode.Nets;

public partial class Network
{
    private partial class LinkCache : Cache<LinkId, LinkCache.Link>
    {
        // // // fields
        private ulong idCounter = 1; // Doesn't start at zero. 1 has a special meaning.

        // // // properties

        public LinkId StimuliFakeId { get; } = LinkId.Secrets.FromIntegerCode(1);

        // // // methods

        public LinkId MakeNew<TIn, TOut>(Network owner, Net<TIn> src, Sub scope, IEnumerable<Net<TOut>> dsts, Func<Voltage<TIn>, Voltage<TOut>> func)
        where TIn : IEquatable<TIn>
        where TOut : IEquatable<TOut>
        {
            LinkId linkId = LinkId.Secrets.FromIntegerCode(Interlocked.Increment(ref idCounter));
            var link = new Link<TIn, TOut>(owner, linkId, scope, src, dsts, func);

            if (owner.nodes.TryGet(src).ThenDont(out NodeCache.Node<TIn>? srcNode))
                throw new($"The source node {src} could not be found");

            owner.Push(new VoltagePush<TIn>(src, linkId, srcNode.GetImmediateSample()));

            return link.Id;
        }

        public bool TryRemoveAndDispose(LinkId linkId)
        {
            if (cache.Remove(linkId, out Link? found))
            {
                found.Dispose();
                return true;
            }
            else return false;
        }
    }
}