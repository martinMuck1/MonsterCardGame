using NUnit.Framework;
using MonsterCardGame;
using MonsterCardGame.Database;
using MonsterCardGame.Server;

namespace MonsterCardGame.Test
{
    public class TestUserDao
    {
        UserDao _userdao;
        Database.Database _db;
        [SetUp]
        public void Setup()
        {
            _userdao = new UserDao();
            _db = Database.Database.getInstance();
        }


        [TestCase("admin", "istrator")]
        public void TestLoginUser_shouldPassLogin(string username, string password)
        {
            _userdao = new UserDao();
            int ret = _userdao.LoginUser(new UserModel(username, Session.ComputeSha256Hash(password)));
            Assert.IsTrue( ret == 0);
        }

        [TestCase("admin", "falschesPw")]
        [TestCase("existiertNicht", "istrator")]
        [TestCase("existiertNicht", "something OR 1=1")]
        public void TestLoginUser_shouldFailLogin(string username, string password)
        {
            int ret = _userdao.LoginUser(new UserModel(username, Session.ComputeSha256Hash(password)));
            Assert.IsTrue( ret == -2);
        }

    }
}