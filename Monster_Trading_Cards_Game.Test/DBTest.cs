using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Monster_Trading_Cards_Game;
using Monster_Trading_Cards_Game.Cards;
using Monster_Trading_Cards_Game.Database;

namespace Monster_Trading_Cards_Game.Test
{
    class DBTest
    {
        DB database = DB.getInstance();
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void getUserPW_Test()
        {
            //act
            database.Connect();
            string password = database.getUserPW("test");
            database.Disconnect();
            //assert
            Assert.AreEqual("123", password);
        }

        [Test]
        public void userExists_returns_true()
        {
            //act
            database.Connect();
            bool exists = database.userExists("test");
            database.Disconnect();
            //assert
            Assert.AreEqual(true, exists);
        }

        [Test]
        public void userExists_returns_false()
        {
            //act
            database.Connect();
            bool exists = database.userExists("tooLongUsernameToExist");
            database.Disconnect();
            //assert
            Assert.AreEqual(false, exists);
        }
    }
}
