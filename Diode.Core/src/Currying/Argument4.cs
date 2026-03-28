namespace Diode;

public record Argument4<T4>(T4 V4)
{
    public Func<T1, T2, T3, T5, TY> IntoQuintary<T1, T2, T3, T5, TY>(Func<T1, T2, T3, T4, T5, TY> func) => (V1, V2, V3, V5) => func(V1, V2, V3, V4, V5);

    public Func<T1, T2, T3, TY> IntoQuartary<T1, T2, T3, TY>(Func<T1, T2, T3, T4, TY> func) => (V1, V2, V3) => func(V1, V2, V3, V4);

    public Action<T1, T2, T3, T5> IntoQuintary<T1, T2, T3, T5>(Action<T1, T2, T3, T4, T5> act) => (V1, V2, V3, V5) => act(V1, V2, V3, V4, V5);

    public Action<T1, T2, T3> IntoQuartary<T1, T2, T3>(Action<T1, T2, T3, T4> act) => (V1, V2, V3) => act(V1, V2, V3, V4);
}



