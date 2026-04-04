namespace Diode.Nets;

public interface IHaveGeneratorNet<TGenerator, TGenKey>
where TGenerator : unmanaged, IGenerator, IEquatable<TGenerator>
where TGenKey : unmanaged, IEquatable<TGenKey>
{
    public Net<TGenerator> GeneratorNet { get; }
}
