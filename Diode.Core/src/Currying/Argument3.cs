namespace Diode;

public record Argument3<T3>(T3 V3)
{
    public Func<T1, T2, T4, T5, TY> IntoQuintary<T1, T2, T4, T5, TY>(Func<T1, T2, T3, T4, T5, TY> func) => (V1, V2, V4, V5) => func(V1, V2, V3, V4, V5);

    public Func<T1, T2, T4, TY> IntoQuartary<T1, T2, T4, TY>(Func<T1, T2, T3, T4, TY> func) => (V1, V2, V4) => func(V1, V2, V3, V4);

    public Func<T1, T2, TY> IntoTernary<T1, T2, TY>(Func<T1, T2, T3, TY> func) => (V1, V2) => func(V1, V2, V3);

    public Action<T1, T2, T4, T5> IntoQuintary<T1, T2, T4, T5>(Action<T1, T2, T3, T4, T5> act) => (V1, V2, V4, V5) => act(V1, V2, V3, V4, V5);

    public Action<T1, T2, T4> IntoQuartary<T1, T2, T4>(Action<T1, T2, T3, T4> act) => (V1, V2, V4) => act(V1, V2, V3, V4);

    public Action<T1, T2> IntoTernary<T1, T2>(Action<T1, T2, T3> act) => (V1, V2) => act(V1, V2, V3);
}



