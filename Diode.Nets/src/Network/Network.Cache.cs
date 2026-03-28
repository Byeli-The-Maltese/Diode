namespace Diode.Nets;

partial class Network
{
    private abstract partial class Cache<TKey, TItem> : IDisposable
    where TItem : Cache<TKey,TItem>.CacheItem
    where TKey : unmanaged
    {
        // // // fields
        protected readonly Dictionary<TKey, TItem> cache = [];
        private bool alreadyDisposed = false;

        // // // methods

        public void Dispose()
        {
            if (alreadyDisposed) return;
            alreadyDisposed = true;
            foreach (var item in cache.Values.ToArray())
                item.Dispose();
            cache.Clear();
        }

        public Rs<TItem> TryGet(TKey key)
            => cache.TryGetValue(key, out TItem? found)
            ? found
            : new Er("Could not find item");
    }

}
