namespace Diode;

public static class Func4Ext
{
    public static Func<T1, T2, T3, TY> Fix4<T1, T2, T3, T4, TY>(this Func<T1, T2, T3, T4, TY> func, T4 V4) => (V1, V2, V3) => func(V1, V2, V3, V4);

    public static Func<T1, T2, T4, TY> Fix3<T1, T2, T3, T4, TY>(this Func<T1, T2, T3, T4, TY> func, T3 V3) => (V1, V2, V4) => func(V1, V2, V3, V4);

    public static Func<T1, T3, T4, TY> Fix2<T1, T2, T3, T4, TY>(this Func<T1, T2, T3, T4, TY> func, T2 V2) => (V1, V3, V4) => func(V1, V2, V3, V4);

    public static Func<T2, T3, T4, TY> Fix1<T1, T2, T3, T4, TY>(this Func<T1, T2, T3, T4, TY> func, T1 V1) => (V2, V3, V4) => func(V1, V2, V3, V4);

    public static Action<T1, T2, T3> Fix4<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> act, T4 V4) => (V1, V2, V3) => act(V1, V2, V3, V4);

    public static Action<T1, T2, T4> Fix3<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> act, T3 V3) => (V1, V2, V4) => act(V1, V2, V3, V4);

    public static Action<T1, T3, T4> Fix2<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> act, T2 V2) => (V1, V3, V4) => act(V1, V2, V3, V4);

    public static Action<T2, T3, T4> Fix1<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> act, T1 V1) => (V2, V3, V4) => act(V1, V2, V3, V4);


    public static Action<T1, T2, T3, T4> DiscardOutput<T1, T2, T3, T4, TY>(this Func<T1, T2, T3, T4, TY> func) => (V1, V2, V3, V4) => func(V1, V2, V3, V4);
}



