namespace Diode.Nets;

public partial class Network
{
    private partial class SubCache : Cache<Sub, SubCache.Subcircuit>
    {
        // // // fields
        private ulong idCounter = 0;

        // // // methods

        public Sub MakeNew(Network owner, SpiceName name, object circuit, string? prefixOfNamespace, Sub scope)
        {
            Sub sub = Sub.Secrets.FromIntegerCode(Interlocked.Increment(ref idCounter));

            var innerSpace = new SpiceSpace(
                string.IsNullOrEmpty(prefixOfNamespace)
                ? name
                : $"{prefixOfNamespace}{SpiceName.SubSep}{name}"
                );

            var subcircuit = new Subcircuit(owner, sub, name, innerSpace, circuit, scope);
            cache[sub] = subcircuit;
            return sub;
        }
    }
}
