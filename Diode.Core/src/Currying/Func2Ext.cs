namespace Diode;

public static class Func2Ext
{
    public static Func<T1, TY> Fix2<T1, T2, TY>(this Func<T1, T2, TY> func, T2 V2) => (V1) => func(V1, V2);

    public static Func<T2, TY> Fix1<T1, T2, TY>(this Func<T1, T2, TY> func, T1 V1) => (V2) => func(V1, V2);

    public static Action<T1> Fix2<T1, T2>(this Action<T1, T2> act, T2 V2) => (V1) => act(V1, V2);

    public static Action<T2> Fix1<T1, T2>(this Action<T1, T2> act, T1 V1) => (V2) => act(V1, V2);


    public static Action<T1, T2> DiscardOutput<T1, T2, TY>(this Func<T1, T2, TY> func) => (V1, V2) => func(V1, V2);
}



