using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Server
{
    class Request
    {
        enum requestType { 
            GET,POST,PUT,DELETE
        }

        requestType reqMethod;
        StreamReader reqReader; 
        
        public Request(StreamReader reader) //handle input from client
        {
            reqReader = reader;
            string[] getRequest = reqReader.ReadLine().Split(" ");
            
            //get Content length
            int contentLength = GetContentLength();
 
        }

        private int GetContentLength() 
        {
            //attention: this does not have to be request 
            string line;
            int contentLength = 0;
            while (!string.IsNullOrWhiteSpace(line = reqReader.ReadLine()))
            {
                if (line.ToLower().StartsWith("content-length:"))
                {
                    contentLength = int.Parse(line.Substring(15).Trim());
                    break;
                }
            }
            //TODO Errorhandling for too long or short or whatever
            return contentLength;
        }
    }
}
