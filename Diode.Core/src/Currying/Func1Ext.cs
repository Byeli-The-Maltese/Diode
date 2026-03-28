namespace Diode;

public static class Func1Ext
{
    public static Func<TY> ToNullary<T1, TY>(this Func<T1, TY> func, T1 V1) => () => func(V1);

    public static Action ToSubroutine<T1>(this Action<T1> act, T1 V1) => () => act(V1);


    public static Action<T1> DiscardOutput<T1, TY>(this Func<T1, TY> func) => (V1) => func(V1);
}



