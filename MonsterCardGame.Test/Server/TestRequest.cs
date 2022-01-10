using NUnit.Framework;
using MonsterCardGame;
using MonsterCardGame.Server;
using Moq;
using System.IO;
using System.Collections.Generic;

namespace MonsterCardGame.Test
{
    class TestRequest
    {
        Request _obj;

        [SetUp]
        public void Setup()
        {
            var readStream = new MemoryStream();
            var writer = new StreamWriter(readStream) { AutoFlush = true };
            writer.WriteLine("POST /sessions/martin HTTP/1.1");
            writer.WriteLine("Content-Type: application/json");
            writer.WriteLine("Authorization: Basic altenhof-mtcgToken");
            writer.WriteLine("Content-Length: 44");
            writer.WriteLine("");
            writer.WriteLine("{\"Username\":\"kienboec\", \"Password\":\"daniel\"}");
            readStream.Position = 0;
            var reader = new StreamReader(readStream);
            _obj = new Request(reader);
        }
        
        [Test]
        public void GetRequestHeader_ShoulReturnSuccess()
        {
            var ret = _obj.GetRequestHeader();

            Assert.IsTrue(ret == responseType.OK);
        }

        [Test]
        public void GetRequestHeader_ShouldSetDictionary()
        {
            Dictionary<string, string> expectedDic = new Dictionary<string, string>();
            expectedDic.Add("Path", "sessions/martin");
            expectedDic.Add("Version", "HTTP/1.1");
            expectedDic.Add("Authorization", "altenhof-mtcgToken");
            expectedDic.Add("ContentLength", "44");
            requestType expectedMethod = requestType.POST;

            _obj.GetRequestHeader();

            CollectionAssert.AreEquivalent(expectedDic, _obj.Header);
            Assert.IsTrue(_obj.ReqMethod == expectedMethod);
        }

        [Test]
        public void GetRequestBody_shouldReturnInput()
        {
            string input = "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}";
            int contentLength = 44;
            _obj.GetRequestHeader();

            var ret = _obj.ReadHttpBody(contentLength).Trim();
            
            StringAssert.AreEqualIgnoringCase(input, ret);
        }
    }
}
