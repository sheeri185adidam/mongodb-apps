using System.Collections.Generic;

namespace Collections
{
    /// <summary>
    /// Interface for key value collection
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IKeyValueCollection<TKey, TValue> : ICollection
    {
        /// <summary>
        /// Gets an enumerable to the keys in the collection
        /// </summary>
        IEnumerable<TKey> Keys{get;}
        
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        IEnumerable<TValue> Values{get;}

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        TValue this[TKey key] {get;set;}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Add(TKey key, TValue value);

        /// <summary>
        /// 
        /// </summary>
        void Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ContainsKey(TKey key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Remove(TKey key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGetValue(TKey key, out TValue value);
    }
}