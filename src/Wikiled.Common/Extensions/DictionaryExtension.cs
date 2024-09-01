using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Wikiled.Common.Extensions
{
    public static class DictionaryExtension
    {
        public static bool TryGetAddItem<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue value, out TValue existingValue)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (!dictionary.TryGetValue(key, out existingValue))
            {
                existingValue = value;
                if (!dictionary.TryAdd(key, value))
                {
                    if (!dictionary.TryGetValue(key, out existingValue))
                    {
                        existingValue = value;
                        return false;
                    }
                }
            }

            return true;
        }

        public static Dictionary<TKey, TValue> Compact<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            var anotherTable = new Dictionary<TKey, TValue>(dictionary.Count, dictionary.Comparer);
            foreach (var word in dictionary)
            {
                anotherTable[word.Key] = word.Value;
            }

            return anotherTable;    
        }

        public static TValue GetItemCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
            where TValue : new()
        {
            if (dictionary.TryGetValue(key, out TValue value))
            {
                return value;
            }

            value = new TValue();
            dictionary[key] = value;
            return value;
        }

        /// <summary>
        /// Given helper handles even if dictionry is null
        /// </summary>
        /// <typeparam name="T">Dictionary key type</typeparam>
        /// <typeparam name="TK">Dictionary value type</typeparam>
        /// <param name="dictionary">Dictionary to which given function is attached</param>
        /// <param name="key">Dictionary key</param>
        /// <returns>Dictionary key. default() if not found</returns>
        public static TK GetSafeNullAble<T, TK>(this Dictionary<T, TK> dictionary, T key)
        {
            return dictionary == null ? default(TK) : GetSafe(dictionary, key);
        }

        /// <summary>
        /// Extension method to dictionary to get value without exception
        /// </summary>
        /// <typeparam name="T">Dictionary key type</typeparam>
        /// <typeparam name="TK">Dictionary value type</typeparam>
        /// <param name="dictionary">Dictionary to which given function is attached</param>
        /// <param name="key">Dictionary key</param>
        /// <returns>Dictionary key. default() if not found</returns>
        public static TK GetSafe<T, TK>(this Dictionary<T, TK> dictionary, T key)
        {
            return dictionary.TryGetValue(key, out TK value) ? value : default(TK);
        }

        /// <summary>
        /// Get item from dictionary and if doesn't exist create new
        /// </summary>
        /// <typeparam name="T">Dictionary key type</typeparam>
        /// <typeparam name="TK">Dictionary value type</typeparam>
        /// <param name="dictionary">Dictionary to which given function is attached</param>
        /// <param name="key">Dictionary key</param>
        /// <returns>Dictionary key. If not found it will be creted</returns>
        public static TK GetSafeCreate<T, TK>(this Dictionary<T, TK> dictionary, T key)
            where TK : new()
        {
            return GetSafeCreate(dictionary, key, item => new TK());
        }

        /// <summary>
        /// Get item from dictionary and if doesn't exist create new using supplied delegate 
        /// </summary>
        /// <typeparam name="T">Dictionary key type</typeparam>
        /// <typeparam name="TK">Dictionary value type</typeparam>
        /// <param name="dictionary">Dictionary to which given function is attached</param>
        /// <param name="key">Dictionary key</param>
        /// <param name="evalutor">Delegate to eveluate default value</param>
        /// <returns>Dictionary key. If not found it will be creted</returns>
        public static TK GetSafeCreate<T, TK>(
            this Dictionary<T, TK> dictionary,
            T key,
            Func<T, TK> evalutor)
        {
            if (dictionary.TryGetValue(key, out TK value))
            {
                return value;
            }

            value = evalutor(key);
            dictionary[key] = value;
            return value;
        }

        /// <summary>
        /// Get item from dictionary and if doesn't exist create new using supplied delegate 
        /// </summary>
        /// <typeparam name="T">Dictionary key type</typeparam>
        /// <typeparam name="TK">Dictionary value type</typeparam>
        /// <param name="dictionary">Dictionary to which given function is attached</param>
        /// <param name="key">Dictionary key</param>
        /// <param name="evalutor">Delegate to eveluate default value</param>
        /// <returns>Dictionary key. If not found it will be creted</returns>
        public static TK GetSafeCreate<T, TK>(
            this Dictionary<T, TK> dictionary,
            T key,
            Func<TK> evalutor)
        {
            if (dictionary.TryGetValue(key, out TK value))
            {
                return value;
            }

            value = evalutor();
            dictionary[key] = value;
            return value;
        }

        /// <summary>
        /// Given helper handles even if dictionry is null
        /// </summary>
        /// <typeparam name="T">Dictionary key type</typeparam>
        /// <typeparam name="TK">Dictionary value type</typeparam>
        /// <param name="dictionary">Dictionary to which given function is attached</param>
        /// <param name="key">Dictionary key</param>
        /// <returns>Dictionary key. default() if not found</returns>
        public static TK GetSafeNullAble<T, TK>(this IDictionary<T, TK> dictionary, T key)
        {
            return dictionary == null ? default(TK) : GetSafe(dictionary, key);
        }

        /// <summary>
        /// Extension method to dictionary to get value without exception
        /// </summary>
        /// <typeparam name="T">Dictionary key type</typeparam>
        /// <typeparam name="TK">Dictionary value type</typeparam>
        /// <param name="dictionary">Dictionary to which given function is attached</param>
        /// <param name="key">Dictionary key</param>
        /// <returns>Dictionary key. default() if not found</returns>
        public static TK GetSafe<T, TK>(this IDictionary<T, TK> dictionary, T key)
        {
            return dictionary.TryGetValue(key, out TK value) ? value : default(TK);
        }

        /// <summary>
        /// Extension method to dictionary to get value without exception and delete if it existed 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TK GetSafeDelete<T, TK>(this IDictionary<T, TK> dictionary, T key)
        {
            if (dictionary.TryGetValue(key, out TK value))
            {
                dictionary.Remove(key);
                return value;
            }

            return default(TK);
        }

        /// <summary>
        /// Get item from dictionary and if doesn't exist create new
        /// </summary>
        /// <typeparam name="T">Dictionary key type</typeparam>
        /// <typeparam name="TK">Dictionary value type</typeparam>
        /// <param name="dictionary">Dictionary to which given function is attached</param>
        /// <param name="key">Dictionary key</param>
        /// <returns>Dictionary key. If not found it will be creted</returns>
        public static TK GetSafeCreate<T, TK>(this IDictionary<T, TK> dictionary, T key)
            where TK : new()
        {
            return GetSafeCreate(dictionary, key, item => new TK());
        }

        /// <summary>
        /// Get item from dictionary and if doesn't exist create new using supplied delegate 
        /// </summary>
        /// <typeparam name="T">Dictionary key type</typeparam>
        /// <typeparam name="TK">Dictionary value type</typeparam>
        /// <param name="dictionary">Dictionary to which given function is attached</param>
        /// <param name="key">Dictionary key</param>
        /// <param name="evalutor">Delegate to eveluate default value</param>
        /// <returns>Dictionary key. If not found it will be creted</returns>
        public static TK GetSafeCreate<T, TK>(
            this IDictionary<T, TK> dictionary,
            T key,
            Func<T, TK> evalutor)
        {
            if (dictionary.TryGetValue(key, out TK value))
            {
                return value;
            }

            value = evalutor(key);
            dictionary[key] = value;
            return value;
        }

        /// <summary>
        /// et item from dictionary and if doesn't exist create new using supplied delegate 
        /// </summary>
        /// <typeparam name="T">Dictionary key type</typeparam>
        /// <typeparam name="TK">Dictionary value type</typeparam>
        /// <param name="dictionary">Dictionary to which given function is attached</param>
        /// <param name="key">Dictionary key</param>
        /// <param name="evalutor">Delegate to eveluate default value</param>
        /// <returns>Dictionary key. If not found it will be creted</returns>
        public static TK GetSafeCreate<T, TK>(
            this IDictionary<T, TK> dictionary,
            T key,
            Func<TK> evalutor)
        {
            if (dictionary.TryGetValue(key, out TK value))
            {
                return value;
            }

            value = evalutor();
            dictionary[key] = value;
            return value;
        }
    }
}
