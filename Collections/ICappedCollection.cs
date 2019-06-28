using System;
using System.Collections.Generic;

namespace Collections
{
    /// <summary>
    /// Interface for capped collection
    /// </summary>
    /// <typeparam name="TValue">A generic type</typeparam>
    public interface ICappedCollection<TValue> : ICollection
    {
        /// <summary>
        /// Number of documents in the collection
        /// </summary>
        int Count {get;}

        /// <summary>
        /// Gets the value at the specified index
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="value">Value at index</param>
        /// <returns>True if value at the index exists.False, otherwise.</returns>
        bool GetValue(int index, out TValue value);

        /// <summary>
        /// Gets or creates the value at the specified index.
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="value">Value at index if it exists.</param>
        /// <param name="creator">A value creator function</param>
        /// <returns></returns>
        bool GetOrCreateValue(int index, out TValue value, Func<TValue> creator);

        /// <summary>
        /// Gets all the values in the collection
        /// </summary>
        /// <returns>An enumerable to all the values</returns>
        IEnumerable<TValue> GetValues();

        /// <summary>
        /// Gets the values within the specified indexes.
        /// </summary>
        /// <param name="from">Starting index</param>
        /// <param name="to">Ending index</param>
        /// <returns>An enumerable to all the values within the indexes.</returns>
        IEnumerable<TValue> GetValues(int from, int to);

        /// <summary>
        /// Sets a value at the index
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="value">Value</param>
        /// <returns>True if successful. False, otherwise.</returns>
        bool SetValue(int index, TValue value);
    }
}
