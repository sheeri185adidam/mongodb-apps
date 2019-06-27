using System;
using System.Collections.Generic;
using System.Linq;
using Collections;
using MongoDB.Driver;
using NUnit.Framework;

namespace Tests
{
    public class TestKeyType
    {
        public string Name {get;set;}
        public int Code {get;set;}
    }

    [TestFixture]
    public class KeyValueCollectionTests
    {
        private const string DatabaseName = @"CollectionsTestsDb";
        private const string CollectionName = @"KeyValueCollection";

        IMongoClient _client;
        IMongoDatabase _database;

        [SetUp]
        public void Setup()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase(DatabaseName);
        }

        [TearDown]
        public void TearDown()
        {
            _client.DropDatabase(DatabaseName);
        }

        [Test]
        public void TestConstructor_AllArgumentsAreNull()
        {
            Assert.Throws<ArgumentNullException>(()=>new KeyValueCollection<string, JackpotTransaction>(null, null));
        }

        [Test]
        public void TestConstructor_DatabaseArgumentIsNull()
        {
            Assert.Throws<ArgumentNullException>(()=>new KeyValueCollection<string, JackpotTransaction>(@"TestCollection", null));
        }

        [Test]
        public void TestConstructor_ValidArguments()
        {
            Assert.DoesNotThrow(()=>new KeyValueCollection<string, JackpotTransaction>(@"TestCollection", _database), null);
        }

        [Test]
        public void TestAdd_SingleKeyValue()
        {
            var key = @"TestKey";
            var collection = new KeyValueCollection<string, JackpotTransaction>(@"TestCollection", _database);

            var transaction = JackpotTransaction.Randomize(new Random());
            collection.Add(key, transaction);

            Assert.IsTrue(collection.TryGetValue(key, out var result));
            Assert.AreEqual(transaction, result);
        }

        [Test]
        public void TestAdd_MultipleKeyValues()
        {
            var key1 = @"TestKey1";
            var key2 = @"TestKey2";
            var key3 = @"TestKey3";

            var collection = new KeyValueCollection<string, JackpotTransaction>(@"TestCollection", _database);

            var transaction1 = JackpotTransaction.Randomize(new Random());
            var transaction2 = JackpotTransaction.Randomize(new Random());
            var transaction3 = JackpotTransaction.Randomize(new Random());

            collection.Add(key1, transaction1);
            collection.Add(key2, transaction2);
            collection.Add(key3, transaction3);

            Assert.IsTrue(collection.TryGetValue(key1, out var result1));
            Assert.IsTrue(collection.TryGetValue(key2, out var result2));
            Assert.IsTrue(collection.TryGetValue(key3, out var result3));

            Assert.AreEqual(transaction1, result1);
            Assert.AreEqual(transaction2, result2);
            Assert.AreEqual(transaction3, result3);
        }

        [Test]
        public void TestAdd_DuplicateKeyValue()
        {
            var key = @"TestKey";
            var collection = new KeyValueCollection<string, JackpotTransaction>(@"TestCollection", _database);

            var transaction = JackpotTransaction.Randomize(new Random());
            collection.Add(key, transaction);

            Assert.Throws<ArgumentException>(()=>collection.Add(key, transaction));
        }

        [Test]
        public void TestClear_EmptyCollection()
        {
            var collection = new KeyValueCollection<string, JackpotTransaction>(@"TestCollection", _database);
            collection.Clear();
        }

        [Test]
        public void TestClear_NonEmptyCollection()
        {
            var key1 = @"TestKey1";
            var key2 = @"TestKey2";
            var key3 = @"TestKey3";

            var collection = new KeyValueCollection<string, JackpotTransaction>(@"TestCollection", _database);

            var transaction1 = JackpotTransaction.Randomize(new Random());
            var transaction2 = JackpotTransaction.Randomize(new Random());
            var transaction3 = JackpotTransaction.Randomize(new Random());

            collection.Add(key1, transaction1);
            collection.Add(key2, transaction2);
            collection.Add(key3, transaction3);

            collection.Clear();

            Assert.IsFalse(collection.TryGetValue(key1, out var result1));
            Assert.IsNull(result1);

            Assert.IsFalse(collection.TryGetValue(key2, out var result2));
            Assert.IsNull(result2);

            Assert.IsFalse(collection.TryGetValue(key3, out var result3));
            Assert.IsNull(result3);
        }

        [Test]
        public void TestContainsKey_SingleKeyExists()
        {
            var key = @"TestKey";
            var collection = new KeyValueCollection<string, JackpotTransaction>(@"TestCollection", _database);

            var transaction = JackpotTransaction.Randomize(new Random());
            collection.Add(key, transaction);

            Assert.IsTrue(collection.ContainsKey(key));
        }

        [Test]
        public void TestContainsKey_MultipleKeysExist()
        {
            var key1 = @"TestKey1";
            var key2 = @"TestKey2";
            var key3 = @"TestKey3";

            var collection = new KeyValueCollection<string, JackpotTransaction>(@"TestCollection", _database);

            var transaction1 = JackpotTransaction.Randomize(new Random());
            var transaction2 = JackpotTransaction.Randomize(new Random());
            var transaction3 = JackpotTransaction.Randomize(new Random());

            collection.Add(key1, transaction1);
            collection.Add(key2, transaction2);
            collection.Add(key3, transaction3);

            Assert.IsTrue(collection.ContainsKey(key1));
            Assert.IsTrue(collection.ContainsKey(key2));
            Assert.IsTrue(collection.ContainsKey(key3));
        }

        [Test]
        public void TestRemoveKey_EmptyCollection()
        {
            var key = @"TestKey";
            var collection = new KeyValueCollection<string, JackpotTransaction>(@"TestCollection", _database);
            Assert.IsFalse(collection.Remove(key));
        }

        [Test]
        public void TestRemoveKey_KeyExists()
        {
            var key = @"TestKey";
            var collection = new KeyValueCollection<string, JackpotTransaction>(@"TestCollection", _database);
            var transaction = JackpotTransaction.Randomize(new Random());
            collection.Add(key, transaction);

            Assert.IsTrue(collection.Remove(key));
        }

        [Test]
        public void TestRemoveKey_KeyDoesNotExist()
        {
            var key = @"TestKey";
            var collection = new KeyValueCollection<string, JackpotTransaction>(@"TestCollection", _database);
            Assert.IsFalse(collection.Remove(key));
        }

        [Test]
        public void TestIndexer_EmptyCollection()
        {
            var key = @"TestKey";
            var collection = new KeyValueCollection<string, JackpotTransaction>(@"TestCollection", _database);
            Assert.Throws<KeyNotFoundException>(() => 
            {
                var result = collection[key];
            });
        }

        [Test]
        public void TestIndexer_SingleKeyValue()
        {
            var key = @"TestKey";
            var collection = new KeyValueCollection<string, JackpotTransaction>(@"TestCollection", _database);
            var transaction = JackpotTransaction.Randomize(new Random());

            collection[key] = transaction;
            var result = collection[key];
            Assert.AreEqual(transaction, result);
        }

        [Test]
        public void TestIndexer_MultipleKeyValues()
        {
            var key1 = @"TestKey1";
            var key2 = @"TestKey2";
            var key3 = @"TestKey3";

            var collection = new KeyValueCollection<string, JackpotTransaction>(@"TestCollection", _database);

            var transaction1 = JackpotTransaction.Randomize(new Random());
            var transaction2 = JackpotTransaction.Randomize(new Random());
            var transaction3 = JackpotTransaction.Randomize(new Random());

            collection[key1] = transaction1;
            collection[key2] = transaction2;
            collection[key3] = transaction3;

            Assert.AreEqual(transaction1, collection[key1]);
            Assert.AreEqual(transaction2, collection[key2]);
            Assert.AreEqual(transaction3, collection[key3]);
        }

        [Test]
        public void TestKeys()
        {
            var key1 = @"TestKey1";
            var key2 = @"TestKey2";
            var key3 = @"TestKey3";

            var collection = new KeyValueCollection<string, JackpotTransaction>(@"TestCollection", _database);

            var transaction1 = JackpotTransaction.Randomize(new Random());
            var transaction2 = JackpotTransaction.Randomize(new Random());
            var transaction3 = JackpotTransaction.Randomize(new Random());

            collection[key1] = transaction1;
            collection[key2] = transaction2;
            collection[key3] = transaction3;

            Assert.AreEqual(3, collection.Keys.Count());
            Assert.IsTrue(collection.Keys.Contains(key1));
            Assert.IsTrue(collection.Keys.Contains(key2));
            Assert.IsTrue(collection.Keys.Contains(key3));
        }

        [Test]
        public void TestValues()
        {
            var key1 = @"TestKey1";
            var key2 = @"TestKey2";
            var key3 = @"TestKey3";

            var collection = new KeyValueCollection<string, JackpotTransaction>(@"TestCollection", _database);

            var transaction1 = JackpotTransaction.Randomize(new Random());
            var transaction2 = JackpotTransaction.Randomize(new Random());
            var transaction3 = JackpotTransaction.Randomize(new Random());

            collection[key1] = transaction1;
            collection[key2] = transaction2;
            collection[key3] = transaction3;

            Assert.AreEqual(3, collection.Values.Count());
            Assert.IsTrue(collection.Values.Contains(transaction1));
            Assert.IsTrue(collection.Values.Contains(transaction2));
            Assert.IsTrue(collection.Values.Contains(transaction3));
        }

        [Test]
        public void TestCustomKeyType()
        {
            var key = new TestKeyType{ Name= @"mongo", Code=100};
            var collection = new KeyValueCollection<TestKeyType, JackpotTransaction>(@"TestCollection", _database);
        }
    }
}