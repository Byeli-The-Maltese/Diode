namespace Diode;

public record Argument1<T1>(T1 V1)
{
    public Func<T2, T3, T4, T5, TY> IntoQuintary<T2, T3, T4, T5, TY>(Func<T1, T2, T3, T4, T5, TY> func) => (V2, V3, V4, V5) => func(V1, V2, V3, V4, V5);

    public Func<T2, T3, T4, TY> IntoQuartary<T2, T3, T4, TY>(Func<T1, T2, T3, T4, TY> func) => (V2, V3, V4) => func(V1, V2, V3, V4);

    public Func<T2, T3, TY> IntoTernary<T2, T3, TY>(Func<T1, T2, T3, TY> func) => (V2, V3) => func(V1, V2, V3);

    public Func<T2, TY> IntoBinary<T2, TY>(Func<T1, T2, TY> func) => (V2) => func(V1, V2);

    public Func<TY> ToNullary<TY>(Func<T1, TY> func) => () => func(V1);

    public Action<T2, T3, T4, T5> IntoQuintary<T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> act) => (V2, V3, V4, V5) => act(V1, V2, V3, V4, V5);

    public Action<T2, T3, T4> IntoQuartary<T2, T3, T4>(Action<T1, T2, T3, T4> act) => (V2, V3, V4) => act(V1, V2, V3, V4);

    public Action<T2, T3> IntoTernary<T2, T3>(Action<T1, T2, T3> act) => (V2, V3) => act(V1, V2, V3);

    public Action<T2> IntoBinary<T2>(Action<T1, T2> act) => (V2) => act(V1, V2);

    public Action ToSubroutine(Action<T1> act) => () => act(V1);
}



