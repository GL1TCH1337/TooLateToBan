using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooLateToBan.Utils
{
    class LimitedDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
        private Queue<TKey> queue = new Queue<TKey>();
        private int maxSize;

        public LimitedDictionary(int maxSize)
        {
            this.maxSize = maxSize;
        }


        public void Add(TKey key, TValue value)
        {
            if (dictionary.Count >= maxSize)
            {
                TKey oldestKey = queue.Dequeue();
                dictionary.Remove(oldestKey);
            }

            dictionary[key] = value;
            queue.Enqueue(key);
        }

        public TValue Get(TKey key)
        {
            return dictionary[key];
        }

        public int Count()
        {
            return dictionary.Count;
        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }
    }
}
