namespace Diode;

public record Argument2<T2>(T2 V2)
{
    public Func<T1, T3, T4, T5, TY> IntoQuintary<T1, T3, T4, T5, TY>(Func<T1, T2, T3, T4, T5, TY> func) => (V1, V3, V4, V5) => func(V1, V2, V3, V4, V5);

    public Func<T1, T3, T4, TY> IntoQuartary<T1, T3, T4, TY>(Func<T1, T2, T3, T4, TY> func) => (V1, V3, V4) => func(V1, V2, V3, V4);

    public Func<T1, T3, TY> IntoTernary<T1, T3, TY>(Func<T1, T2, T3, TY> func) => (V1, V3) => func(V1, V2, V3);

    public Func<T1, TY> IntoBinary<T1, TY>(Func<T1, T2, TY> func) => (V1) => func(V1, V2);

    public Action<T1, T3, T4, T5> IntoQuintary<T1, T3, T4, T5>(Action<T1, T2, T3, T4, T5> act) => (V1, V3, V4, V5) => act(V1, V2, V3, V4, V5);

    public Action<T1, T3, T4> IntoQuartary<T1, T3, T4>(Action<T1, T2, T3, T4> act) => (V1, V3, V4) => act(V1, V2, V3, V4);

    public Action<T1, T3> IntoTernary<T1, T3>(Action<T1, T2, T3> act) => (V1, V3) => act(V1, V2, V3);

    public Action<T1> IntoBinary<T1>(Action<T1, T2> act) => (V1) => act(V1, V2);
}



