using NUnit.Framework;

namespace Monster_Trading_Cards_Game.Test
{
    public class UserTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_setDeck()
        {
            //arrange
            var input1 = "Test";
            User user = new Monster_Trading_Cards_Game.User("TestUser", "123", );
            //act
            user.setDeck();
            //assert
            Assert.Pass();
        }
    }
}