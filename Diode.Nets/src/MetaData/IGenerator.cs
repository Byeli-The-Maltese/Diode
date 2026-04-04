namespace Diode.Nets;

public interface IGenerator
{
    public SpiceName Key { get; }

    public LifeCycle GetMode();
}
