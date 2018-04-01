using System.Collections.Generic;

namespace Server.Sync
{
    public static class Extensions
    {
        public static void Set<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
                dict[key] = value;
            else
                dict.Add(key, value);
        }

        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            return dict.ContainsKey(key) ? dict[key] : default(TValue);
        }

        public static int Get(this IDictionary<int, int> dict, int key)
        {
            return dict.ContainsKey(key) ? dict[key] : -1;
        }
    }
}