using NUnit.Framework;
using MonsterCardGame;
using MonsterCardGame.Server;
using Moq;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Test
{
    class TestRouting
    {
        Routing _obj;
        [SetUp]
        public void Setup()
        {

        }
        
        [Test]
        public void FindRoute_LoginPath_ShouldSendResponseOK()
        {
            Mock<Response> resp = TestHelper.GetMockResponse();
            var readStream = new MemoryStream();
            var reader = new StreamReader(readStream);
            Mock<Request> req = new Mock<Request>(reader);
            req.Object.Header.Add("Path", "sessions");
            req.Object.Header.Add("ContentLength", "0");
            req.Object.ReqMethod = requestType.POST; 
        
            req
                .Setup(x => x.ReadHttpBody(It.IsAny<int>()))
                .Returns("{\"Username\":\"altenhof\",\"Password\":\"markus\"}");

            _obj = new Routing(req.Object, resp.Object);
            _obj.FindRoute();
            resp.Verify(mock => mock.SendResponse(responseType.OK,It.IsAny<string>()), Times.Once());
        }
        /*
        [Test]
        public void FindRoute_LoginPath()
        {
            Mock<Response> resp = TestHelper.GetMockResponse();
            var readStream = new MemoryStream();
            var reader = new StreamReader(readStream);
            Mock<Request> req = new Mock<Request>(reader);
            req.Object.Header.Add("Path", "sessions");
            req.Object.Header.Add("ContentLength", "0");
            req.Object.ReqMethod = requestType.POST;

            req
                .Setup(x => x.ReadHttpBody(It.IsAny<int>()))
                .Returns("{\"Username\":\"altenhof\",\"Password\":\"markus\"}");

            _obj = new Routing(req.Object, resp.Object);
            _obj.FindRoute();
            resp.Verify(mock => , Times.Once()) ;
        }
        */

    }
}
