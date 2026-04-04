using System.Runtime.CompilerServices;

namespace Diode.Nets;

public partial class Network
{

    public readonly ref partial struct Com<TPort>
    where TPort : unmanaged
    {
        // // // more methods
        public ComPlug<TPlug> Plug<TPlug>(TPlug downwardPort)
        where TPlug : unmanaged
        => new(this, downwardPort)
        ;

        // // // nested types

        public readonly ref struct ComPlug<TPlug>(Com<TPort> com, TPlug plug)
        where TPlug : unmanaged
        {
            // // // fields
            private readonly Com<TPort> com = com;
            private readonly TPlug plug = plug;

            // // // methods

            public Com<TPort> IntoNew<TCircuit>(out Sub newSubId, [CallerArgumentExpression(nameof(newSubId))] string name = "")
            where TCircuit : ICircuit<TPlug>, new()
                => com
                .InternalSub<TCircuit, TPlug>(plug, out newSubId, name, false, null)
                ;

            public ComGenerator<TGeneratorNetValue> AsGenerator<TGeneratorNetValue>(Func<TPlug, Net<TGeneratorNetValue>> generatorNetSelector)
            where TGeneratorNetValue : unmanaged, IGenerator, IEquatable<TGeneratorNetValue>
                => new(this, generatorNetSelector);


            // // // nested types

            public readonly ref struct ComGenerator<TGeneratorNetValue>(ComPlug<TPlug> comPlug, Func<TPlug, Net<TGeneratorNetValue>> generatorNetSelector)
            where TGeneratorNetValue : unmanaged, IGenerator, IEquatable<TGeneratorNetValue>
            {
                // // // fields
                private readonly ComPlug<TPlug> comPlug = comPlug;
                private readonly Func<TPlug, Net<TGeneratorNetValue>> generatorNetSelector = generatorNetSelector;

                // // // methods

                public Com<TPort> IntoExpander<TChild>(out Sub newSubId, [CallerArgumentExpression(nameof(newSubId))] string name = "")
                where TChild : ICircuit<TPlug>, new()
                {
                    var generatorNetSelectorCopy = generatorNetSelector;
                    void SetExpandersGeneratorNetSelector(Expander<TPlug, TGeneratorNetValue, TChild> expander)
                    {
                        expander.GeneratorNetSelector = generatorNetSelectorCopy;
                    }

                    return comPlug.com.InternalSub<Expander<TPlug, TGeneratorNetValue, TChild>, TPlug>(comPlug.plug, out newSubId, name, false, SetExpandersGeneratorNetSelector);
                }
            }


            
        
        }


    }


}
