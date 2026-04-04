namespace Diode.Nets;

public record SpiceName
{
    public const string SubSep = ".";
    public const string SubExpanderSep1 = "<";
    public const string SubExpanderSep2 = ">";
    public const string NetSep = ":";
    public const string LinkSep = "+";

    private SpiceName(string Value) => this.Value = Value;

    public string Value { get; }

    public static Rs<SpiceName> Create(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return new Er("SPICE name cannot be null or whitespace");

        if (raw[0] is 
            not '_' 
            and 
            not (>= 'a' and <= 'z')
            and
            not (>= 'A' and <= 'Z')
            )
            return new Er("SPICE names must begin with an underscore or an (en-us) letter");

        foreach (char c in raw.Skip(1))
            if (c is 
                not '_' 
                and 
                not (>= 'a' and <= 'z') 
                and
                not (>= 'A' and <= 'Z')
                and 
                not '0'
                and 
                not (>= '1' and <= '9')
                )
                return new Er("SPICE names must consist of underscores, (en-us) letters, and digits");

        return new SpiceName(raw);
    }

    public static implicit operator string(SpiceName sn) => sn.Value;

    public override string ToString() => Value;
}