using NUnit.Framework;
using MonsterCardGame;
using MonsterCardGame.Server;
using Moq;
using System.IO;

namespace MonsterCardGame.Test
{
    class TestLoginHandler
    {
        
        [SetUp]
        public void Setup()
        {
            
        }
        
        [TestCase()]
        public void Handle_ShouldSetDictionary()
        {
            var expectedToken = "altenhof-mtcgToken";
            var obj = new Mock<LoginHandle>(AuthLevel.noLogin);
            obj.CallBase = true;
            Mock <Response> resp= TestHelper.GetMockResponse();
            resp.Setup(x => x.SendResponse(It.IsAny<responseType>(), It.IsAny<string>()));

            obj.Object.DeserializeMessage("{\"Username\":\"altenhof\", \"Password\":\"markus\"}");
            obj.Object.Handle(resp.Object, "");

            Assert.IsTrue(Session.SessionDic.ContainsKey(expectedToken) == true);
        }
        

    }
}
