using NUnit.Framework;
using MonsterCardGame;
using MonsterCardGame.Server;
using Moq;
using System.IO;

namespace MonsterCardGame.Test
{
    class TestHandler
    {
        Handler _obj;
        string username = "";
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void CheckAuth_NoLoginNeeded_ShouldPass()
        {
            //no elements in Session Dic needed
            _obj = new UserHandle(AuthLevel.noLogin);
            
            Assert.IsTrue(_obj.CheckAuth(null, "",ref username) == true);
        }

        [TestCase("kienboec", "kienboec-mtcgToken")]
        public void CheckAuth_LoginNeeded_ShouldPass(string username, string token)
        {
            Session.SessionDic.TryAdd(token, username);
            _obj = new UserHandle(AuthLevel.Login);
            Assert.IsTrue(_obj.CheckAuth(null, token,ref username) == true);
            Session.SessionDic.Clear();
        }
        
        [TestCase("kienboec-mctgToken")]
        [TestCase("admin-mctgToken")]
        public void CheckAuth_LoginNeeded_ShouldFail(string token)
        {
            var mockResp = TestHelper.GetMockResponse();
            Session.SessionDic.TryAdd("altenhof-mtcgToken", "altenhof");
            _obj = new UserHandle(AuthLevel.Login);

            Assert.IsTrue(_obj.CheckAuth(mockResp.Object, token,ref username) == false, "Token from user is not in memory");
            Session.SessionDic.Clear();
        }

        [TestCase("admin-mtcgToken")]
        public void CheckAuth_AdminLogin_shouldPass(string token)
        {
            Session.SessionDic.TryAdd("admin-mtcgToken", "admin");
            _obj = new UserHandle(AuthLevel.Admin);

            Assert.IsTrue(_obj.CheckAuth(null, token,ref username) == true);
            Session.SessionDic.Clear();
        }

        [TestCase("altenhof-mctgToken")]
        public void CheckAuth_AdminLogin_shouldFail(string token)
        {
            var mockResp = TestHelper.GetMockResponse();
            Session.SessionDic.TryAdd("altenhof-mtcgToken", "altenhof");
            _obj = new UserHandle(AuthLevel.Admin);

            Assert.IsTrue(_obj.CheckAuth(mockResp.Object, token, ref username) == false);
            Session.SessionDic.Clear();
        }

    }
}
