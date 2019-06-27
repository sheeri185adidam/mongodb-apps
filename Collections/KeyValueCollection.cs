using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Collections 
{
    /// <summary>
    /// A KeyValueCollection class
    /// </summary>
    public class KeyValueCollection<TKey, TValue> : IKeyValueCollection<TKey, TValue>
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<CollectionElement> _collection;

        /// <summary>
        /// A constructor
        /// </summary>
        public KeyValueCollection(string name, IMongoDatabase database)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _collection = _database.GetCollection<CollectionElement>(Name);

            var keys = Builders<CollectionElement>.IndexKeys.Hashed("Key");
            var indexOptions = new CreateIndexOptions{};
            var model  = new CreateIndexModel<CollectionElement>(keys, indexOptions);
            _collection.Indexes.CreateOne(model);
        }


        /// <inheritdoc/>
        public TValue this[TKey key] 
        { 
            get 
            {
                if(TryGetValue(key, out var value))
                {
                    return value;
                }

                throw new KeyNotFoundException(nameof(key));
            } 
            
            set 
            {
                var filter = Builders<CollectionElement>.Filter.Eq("Key", key);
                var update = Builders<CollectionElement>.Update.Set("Value", value);
                _collection.UpdateOne(filter, update, new UpdateOptions {IsUpsert = true});
            }
        }

        public string Name {get; private set;}

        public IEnumerable<TKey> Keys 
        {
            get
            {
                foreach(var document in AllDocuments())
                {
                    yield return document.Key;
                }
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                foreach(var document in AllDocuments())
                {
                    yield return document.Value;
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            if(ContainsKey(key))
            {
                throw new ArgumentException(nameof(key));
            }

            _collection.InsertOne(new CollectionElement 
            {
                Key = key,
                Value = value
            });
        }

        public void Clear()
        {
            _collection.DeleteMany(Builders<CollectionElement>.Filter.Empty);
        }

        public bool ContainsKey(TKey key)
        {
            var filter = Builders<CollectionElement>.Filter.Eq("Key", key);
            var result = _collection.Find(filter).FirstOrDefault();
            if(result == null)
            {
                return false;
            }

            return true;
        }

        public bool Remove(TKey key)
        {
            var filter = Builders<CollectionElement>.Filter.Eq("Key", key);
            var result = _collection.DeleteOne(filter);
            return result.DeletedCount > 0;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var filter = Builders<CollectionElement>.Filter.Eq("Key", key);
            var result = _collection.Find(filter).FirstOrDefault();

            if(result == null)
            {
                value = default(TValue);
                return false;
            }

            value = result.Value;
            return true;
        }

        private IEnumerable<CollectionElement> AllDocuments()
        {
            return _collection.Find(Builders<CollectionElement>.Filter.Empty).ToEnumerable();
        }

        /// <summary>
        /// 
        /// </summary>
        public class CollectionElement
        {
            /// <summary>
            /// 
            /// </summary>
            /// <value></value>
            public ObjectId Id { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <value></value>
            public TKey Key {get; set;}

            /// <summary>
            /// 
            /// </summary>
            /// <value></value>

            public TValue Value {get;set;}
        }
    }
}