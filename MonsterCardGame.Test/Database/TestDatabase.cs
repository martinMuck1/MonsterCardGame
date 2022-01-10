using NUnit.Framework;
using MonsterCardGame;
using MonsterCardGame.Database;
using System;

namespace MonsterCardGame.Test
{
    public class TestDatabase
    {
        Database.Database _db;
        [SetUp]
        public void Setup()
        {
            
        }
        
        [Test]
        public void DatabaseConstructor_shouldEstablishConnection()
        {
            _db = Database.Database.getInstance();
            Assert.IsTrue(_db.Conn.State == System.Data.ConnectionState.Open);
        }
        



    }
}