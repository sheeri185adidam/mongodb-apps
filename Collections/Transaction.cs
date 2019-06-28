using System;
using System.Collections.Concurrent;
using MongoDB.Driver;

namespace Collections
{
    /// <summary>
    /// 
    /// </summary>
    public class Transaction : ITransaction
    {
        private ConcurrentDictionary<string, object> _collections = new ConcurrentDictionary<string, object>();
        private IMongoClient _client;
        private IClientSession _session;

        /// <summary>
        /// A transaction constructor
        /// </summary>
        /// <param name="client"></param>
        public Transaction(IMongoClient client)
        {
            _client = client;
        }

        public void Abort()
        {
            _session?.AbortTransaction();
        }

        public bool Add<TCollection>(TCollection collection)
            where TCollection: ICollection
        {
            if(_collections.ContainsKey(collection.Name))
            {
                return false;
            }

            _collections.TryAdd(collection.Name, collection);
            return true;
        }

        public void Commit()
        {
            _session?.CommitTransaction();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool Get<TCollection>(string name, out TCollection collection)
            where TCollection: ICollection
        {
            if(_collections.TryGetValue(name, out var result))
            {
                collection = (TCollection)result;
                return true;
            }

            collection = default(TCollection);
            return false;
        }

        public bool Start()
        {
            _session = _client.StartSession();
            if(_session == null)
            {
                return false;
            }

            _session.StartTransaction();
            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                _session?.Dispose();
                _session = null;
            }
        }
    }
}