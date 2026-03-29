namespace Diode.Nets;

public interface ICircuit<TPort>
where TPort: unmanaged
{
    public Network.Com<TPort> Install(Network.Com<TPort> com);

    public Network.AccessToken Host { get; init; }
}

public interface ICircuit : ICircuit<None>;