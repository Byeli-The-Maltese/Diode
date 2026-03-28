namespace Diode;

public record Argument5<T5>(T5 V5)
{
    public Func<T1, T2, T3, T4, TY> IntoQuintary<T1, T2, T3, T4, TY>(Func<T1, T2, T3, T4, T5, TY> func) => (V1, V2, V3, V4) => func(V1, V2, V3, V4, V5);

    public Action<T1, T2, T3, T4> IntoQuintary<T1, T2, T3, T4>(Action<T1, T2, T3, T4, T5> act) => (V1, V2, V3, V4) => act(V1, V2, V3, V4, V5);
}



