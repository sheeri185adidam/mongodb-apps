using System;

namespace Collections
{
    /// <summary>
    /// A transaction interface
    /// </summary>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Start the transaction
        /// </summary>
        /// <returns>True if transaction was successfully started.null False, otherwise.</returns>
        bool Start();

        /// <summary>
        /// Commit the transaction
        /// </summary>
        void Commit();

        /// <summary>
        /// Abort the transaction
        /// </summary>
        void Abort();

        /// <summary>
        /// Add a collection to the transaction
        /// </summary>
        /// <param name="collection">A reference to the collection</param>
        /// <typeparam name="TCollection">Collection type</typeparam>
        /// <returns>Reference to the collection added</returns>
        bool Add<TCollection>(TCollection collection) where TCollection: ICollection;

        /// <summary>
        /// Gets a collection from the transaction
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="TCollection"></typeparam>
        /// <returns></returns>
        bool Get<TCollection>(string name, out TCollection value) where TCollection: ICollection;
    }
}