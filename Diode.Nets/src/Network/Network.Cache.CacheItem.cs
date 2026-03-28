namespace Diode.Nets;

partial class Network
{
    private abstract partial class Cache<TKey, TItem> where TItem : Cache<TKey,TItem>.CacheItem
    where TKey : unmanaged
    {
        internal abstract class CacheItem : IDisposable
        {
            // // // fields
            protected readonly Network network;
            private bool alreadyDisposed = false;

            // // // constructor
            public CacheItem(Network network, TKey id, Sub scope) => (this.network, Id, Scope) = (network, id, scope);

            // // // properties

            public TKey Id { get; }

            public Sub Scope { get; }

            // // // methods

            protected abstract Cache<TKey, TItem> GetMyContainer();

            public virtual void Dispose()
            {
                if (alreadyDisposed) return;
                alreadyDisposed = true;
                GetMyContainer().cache.Remove(Id);
            }
        }
    }

}
