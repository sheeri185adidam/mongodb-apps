using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Collections
{
    /// <summary>
    /// A capped collection class
    /// </summary>
    /// <typeparam name="TValue">A generic type</typeparam>
    public class CappedCollection<TValue> : ICappedCollection<TValue>, IDisposable
    {
        private readonly int _maxDocuments;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<CollectionElement> _collection;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The collection name.</param>
        /// <param name="max">Maximum number of elements the collection can contain.</param>
        /// <param name="database">A reference to mongodb database</param>
        public CappedCollection(string name, int max, IMongoDatabase database)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));;
            _maxDocuments = max;
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _collection = _database.GetCollection<CollectionElement>(Name);

            var keys = Builders<CollectionElement>.IndexKeys.Ascending("Index");
            var indexOptions = new CreateIndexOptions{};
            var model  = new CreateIndexModel<CollectionElement>(keys, indexOptions);
            _collection.Indexes.CreateOne(model);
        }

        public int Count => (int)_collection.CountDocuments(FilterDefinition<CollectionElement>.Empty);

        public string Name {get;}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool GetOrCreateValue(int index, out TValue value, Func<TValue> creator)
        {
            if(!CheckBounds(index))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if(GetValue(index, out value))
            {
                return true;
            }

            value = creator();
            _collection.InsertOne(new CollectionElement
            {
                Index = index,
                Value = value
            });
            return true;
        }

        public bool GetValue(int index, out TValue value)
        {
            if(!CheckBounds(index))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var filter = Builders<CollectionElement>.Filter.Eq("Index", index);
            var result = _collection.Find(filter).FirstOrDefault();
            if(result == null)
            {
                value = default(TValue);
                return false;
            }

            value = result.Value;
            return true;
        }

        public IEnumerable<TValue> GetValues()
        {
            var filter = Builders<CollectionElement>.Filter.Empty;
            foreach(var element in _collection.Find(filter).ToEnumerable())
            {
                yield return element.Value;
            }
        }

        public IEnumerable<TValue> GetValues(int from, int to)
        {
            if(!CheckBounds(from))
            {
                throw new ArgumentOutOfRangeException(nameof(from));
            }

            if(!CheckBounds(to))
            {
                throw new ArgumentOutOfRangeException(nameof(to));
            }

            var builder = Builders<CollectionElement>.Filter;
            var filter = builder.AnyGte("Index", from) & builder.AnyLte("Index", to);
            foreach(var element in _collection.Find(filter).ToEnumerable())
            {
                yield return element.Value;
            }
        }

        public bool SetValue(int index, TValue value)
        {
            if(!CheckBounds(index))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var filter = Builders<CollectionElement>.Filter.Eq("Index", index);
            var update = Builders<CollectionElement>.Update.Set("Value", value);
            _collection.UpdateOne(filter, update, new UpdateOptions {IsUpsert = true});
            return true;
        }

        protected void Dispose(bool disposing)
        {
            if(disposing)
            {
                _database.DropCollection(Name);
            }
        }

        private bool CheckBounds(int index)
        {
            if(index<0 || index>=_maxDocuments)
            {
                return false;
            }

            return true;
        }

        internal class CollectionElement
        {
            [BsonId]
            public int Index {get; set;}
            
            public TValue Value {get;set;}
        }
    }
}