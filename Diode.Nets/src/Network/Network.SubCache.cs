namespace Diode.Nets;

public partial class Network
{
    private partial class SubCache : Cache<Sub, SubCache.Subcircuit>
    {
        // // // fields
        private ulong idCounter = 0;

        // // // methods

        public Sub MakeNew(Network owner, SpiceName name, object circuit, string? prefixOfNamespace, Sub scope, bool isForExpander)
        {
            Sub sub = Sub.Secrets.FromIntegerCode(Interlocked.Increment(ref idCounter));

            var innerSpace = new SpiceSpace(
                string.IsNullOrEmpty(prefixOfNamespace)
                ? name
                : isForExpander
                    ? $"{prefixOfNamespace}{SpiceName.SubExpanderSep1}{name}{SpiceName.SubExpanderSep2}"
                    : $"{prefixOfNamespace}{SpiceName.SubSep}{name}"
                );

            var subcircuit = new Subcircuit(owner, sub, name, innerSpace, circuit, scope);
            cache[sub] = subcircuit;
            return sub;
        }
    }
}
