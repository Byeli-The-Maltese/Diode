namespace Diode;

public static class Func3Ext
{
    public static Func<T1, T2, TY> Fix3<T1, T2, T3, TY>(this Func<T1, T2, T3, TY> func, T3 V3) => (V1, V2) => func(V1, V2, V3);

    public static Func<T1, T3, TY> Fix2<T1, T2, T3, TY>(this Func<T1, T2, T3, TY> func, T2 V2) => (V1, V3) => func(V1, V2, V3);

    public static Func<T2, T3, TY> Fix1<T1, T2, T3, TY>(this Func<T1, T2, T3, TY> func, T1 V1) => (V2, V3) => func(V1, V2, V3);

    public static Action<T1, T2> Fix3<T1, T2, T3>(this Action<T1, T2, T3> act, T3 V3) => (V1, V2) => act(V1, V2, V3);

    public static Action<T1, T3> Fix2<T1, T2, T3>(this Action<T1, T2, T3> act, T2 V2) => (V1, V3) => act(V1, V2, V3);

    public static Action<T2, T3> Fix1<T1, T2, T3>(this Action<T1, T2, T3> act, T1 V1) => (V2, V3) => act(V1, V2, V3);


    public static Action<T1, T2, T3> DiscardOutput<T1, T2, T3, TY>(this Func<T1, T2, T3, TY> func) => (V1, V2, V3) => func(V1, V2, V3);
}



