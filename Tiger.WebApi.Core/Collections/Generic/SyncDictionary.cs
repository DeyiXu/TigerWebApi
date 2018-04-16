using System.Collections.Generic;

namespace Tiger.WebApi.Core.Collections.Generic
{
    public class SyncDictionary<K, V> : Dictionary<K, V>
    {
        private object _locker = new object();
        public SyncDictionary() { }

        public SyncDictionary(IDictionary<K, V> dictionary)
            : base(dictionary)
        { }


        public new void Add(K key, V value)
        {
            if (key != null && value != null)
            {
                lock (_locker)
                {
                    base[key] = value;
                }
            }
        }

        public void AddAll(IDictionary<K, V> dict)
        {
            if (dict != null && dict.Count > 0)
            {
                IEnumerator<KeyValuePair<K, V>> kvps = dict.GetEnumerator();
                while (kvps.MoveNext())
                {
                    KeyValuePair<K, V> kvp = kvps.Current;
                    Add(kvp.Key, kvp.Value);
                }
            }
        }

        public new bool Remove(K key)
        {
            lock (_locker)
            {
                if (ContainsKey(key))
                    return base.Remove(key);
            }
            return false;
        }

        public new V this[K key]
        {
            get
            {
                return base[key];
            }
            set
            {
                lock (_locker)
                {
                    base[key] = value;
                }
            }
        }
    }
}
