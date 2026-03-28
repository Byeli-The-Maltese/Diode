namespace Diode.Nets;

public partial class Network
{
    public struct Options
    {
        public bool LogNodeStateChange { get; set; }

        public bool LogNodeLifetimes { get; set; }

        public bool LogLinkDrives { get; set; }

        public bool LogLinkLifetimes { get; set; }

        public bool LogSubcircuitLifetimes { get; set; }

        public bool LogVoltageFlushes { get; set; }

        public bool LogStimuli { get; set; }

        public bool LogWarnings { get; set; }

        public static Options LogAll { get; } = new()
        {
            LogNodeStateChange = true,
            LogNodeLifetimes = true,
            LogLinkDrives = true,
            LogLinkLifetimes = true,
            LogSubcircuitLifetimes = true,
            LogVoltageFlushes = true,
            LogStimuli = true,
            LogWarnings = true
        };
    }
}