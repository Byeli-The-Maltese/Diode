namespace Diode.Nets;

public partial class Network
{
    internal sealed class Expander<TPort, TGeneratorNetValue, TChild> : ICircuit<TPort>, IDisposable
    where TPort : unmanaged
    where TGeneratorNetValue : unmanaged, IGenerator, IEquatable<TGeneratorNetValue>
    where TChild : ICircuit<TPort>, new()
    {
        // // // fields
        private bool alreadyDisposed = false;
        private Sub myScope;
        private TPort myPort;

        // // // properties
        public AccessToken Host { get; init; }

        public Func<TPort, Net<TGeneratorNetValue>>? GeneratorNetSelector { get; set;}


        // // // methods
        public Com<TPort> Install(Com<TPort> com)
            => com
            .GetBuildScope(out myScope)
            .GetPort(out myPort)
            .TapFrom(GetGeneratorNet(com.Port))
            .Through(HandleGenerator)
            .IntoGround()
            ;

        private Net<TGeneratorNetValue> GetGeneratorNet(TPort port)
            => GeneratorNetSelector is null
            ? throw new("KeyingFunction was null. This should not happen")
            : GeneratorNetSelector(port);

        private void DisposeAllChildren()
        {
            if (Host.GetSecret().ThenDont(out Network? network)) return;

            if (network.subs.TryGet(myScope).ThenDont(out SubCache.Subcircuit? expanderSubcircuit)) return;

            Sub[] disposalTargets = expanderSubcircuit.NameSpace.Subs.Values.ToArray();

            foreach (var sub in disposalTargets)
                network.subs.TryGet(sub).Match(s => s.Dispose());
        }

        private Voltage<None> HandleGenerator(Voltage<TGeneratorNetValue> generator)
        {
            if (Host.GetSecret().ThenDont(out Network? network)) goto Abort;
            if (alreadyDisposed) goto Abort;

            if (generator.Resolve().ThenDont(out TGeneratorNetValue command))
            {
                DisposeAllChildren();
                goto Abort;
            }
            
            if (network.subs.TryGet(myScope).ThenDont(out SubCache.Subcircuit? expanderSubcircuit)) goto Abort;
            SpiceName childName = command.Key;
            bool childNameAlreadyPresent = expanderSubcircuit.NameSpace.Subs.ContainsKey(childName);

            switch (command.GetMode())
            {
                case LifeCycle.Create:

                    if (childNameAlreadyPresent) goto Abort; // Abort instead of throwing

                    network
                        .MintTopLevelCommission(myPort)
                        .InternalSub<TChild, TPort>(myPort, out Sub childSub, childName, true, null);

                    break;

                case LifeCycle.Destroy:

                    if (!childNameAlreadyPresent) goto Abort;

                    Sub existingChild = expanderSubcircuit.NameSpace.Subs[childName];

                    network.subs.TryGet(existingChild).Match(s => s.Dispose());

                    break;
                    
                default:
                    throw new($"Unexpected {nameof(LifeCycle)} value");
            }
            
        Abort:
            return default;
        }

        public override string ToString() => "";

        public void Dispose()
        {
            if (alreadyDisposed) return;
            alreadyDisposed = true;
            DisposeAllChildren();
        }
    }
}

/*
// GeneratorIndex is unmanaged and implements IGeneratorIndex
// It has a method that indicates if the value should create or destroy a generated element
// It also has a method that exposes a TKey of type IEquatable<TKey> that is used for keying 
private Net<GeneratorIndex> genIndex;
private Net<TSlot3> slot3;

public Network.Com<Port1> Install(Network.Com<Port1> com) 
    => com
    .Net(out genIndex)
    .Net(out slot3)
    .Generate(genIndex, com2 =>
        com2
        .Net(out Net<TSlot1> slot1)
        .Net(out Net<TSlot2> slot2)
        .Sub(new TPort2( slot1, slot2, slot3, com.Slot4 ), out Sub ChildItem) 
        .TapFrom(slot1) 
        .Through(HandleSlot1) 
        .IntoGround()
    )
    ; 
    
    private void HandleSlot1(Voltage<TSlot1> voltage) 
    {
        ...
    }

*/