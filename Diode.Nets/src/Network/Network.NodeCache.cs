namespace Diode.Nets;

public partial class Network
{
    private partial class NodeCache : Cache<ulong, NodeCache.Node>
    {
        // // // fields
        private ulong idCounter = 0;

        // // // methods

        public Rs<Node<T>> TryGet<T>(Net<T> net)
        where T : IEquatable<T>
            => cache.TryGetValue(net, out Node? value)
            ? value is Node<T> correctValue
                ? correctValue
                : new Er($"The net {net} was used to look up a node, but the node of type {typeof(T)}")
            : new Er($"The net does not exist");

        public Net<T> MakeNew<T>(Network network, SpiceName name, Sub scope)
        where T : IEquatable<T>
        {
            Net<T> net = Net<T>.Secrets.FromIntegerCode(Interlocked.Increment(ref idCounter));
            var node = new Node<T>(name, network, net, scope);
            cache[net] = node;
            return net;
        }
    }


}
