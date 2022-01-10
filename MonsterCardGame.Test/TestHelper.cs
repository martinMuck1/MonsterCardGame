using NUnit.Framework;
using MonsterCardGame;
using MonsterCardGame.Server;
using Moq;
using System.IO;

namespace MonsterCardGame.Test
{
    class TestHelper
    {
       
        [SetUp]
        public void Setup()
        {
            
        }

        public static Mock<Response> GetMockResponse()
        {
            Mock<Response> mockResp;
            var readStream = new MemoryStream();
            var writer = new StreamWriter(readStream) { AutoFlush = true };
            writer.Write("");
            mockResp = new Mock<Response>(writer);
            //mockResp.Setup(((mock)=> mock.SendResponse(responseType.UNAUTHORIZED, "{\"message\": \"access denied\"}")));
            
            return mockResp;
        }

        
        
    }
}
