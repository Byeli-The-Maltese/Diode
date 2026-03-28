namespace Diode.Nets;

public partial class Network
{
    public ref partial struct Com<TPort>
    {
        public RouteFrom<TSrc> TapFrom<TSrc>(Net<TSrc> net)
        where TSrc : IEquatable<TSrc>
            => new(this, net);

        public readonly ref struct RouteFrom<TSrc>
        where TSrc : IEquatable<TSrc>
        {
            internal readonly Com<TPort> com;
            internal readonly Net<TSrc> net;

            public RouteFrom(Com<TPort> com, Net<TSrc> net)
            {
                this.com = com;
                this.net = net;
            }
            public RouteThru<TSrc, TU> Through<TU>(Func<Voltage<TSrc>, Voltage<TU>> func) where TU : IEquatable<TU> => new(this, func);
        }

        public readonly ref struct RouteThru<TSrc, TDst>
        where TSrc : IEquatable<TSrc>
        where TDst : IEquatable<TDst>
        {
            private readonly RouteFrom<TSrc> src;
            private readonly Func<Voltage<TSrc>, Voltage<TDst>> xform;

            public RouteThru(RouteFrom<TSrc> src, Func<Voltage<TSrc>, Voltage<TDst>> xform)
            {
                this.src = src;
                this.xform = xform;
            }

            public Com<TPort> IntoGround() => OntoNets();

            public Com<TPort> OntoNet(Net<TDst> target) => OntoNets(target);

            public Com<TPort> OntoNets(params IEnumerable<Net<TDst>> targets)
            {
                src.com.Authority.links.MakeNew(src.com.Authority, src.net, src.com.buildScope, targets, xform);
                return src.com;
            }
        }
    }
}
