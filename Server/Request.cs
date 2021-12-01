using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Server
{   public enum responseType{
        OK, ERR, TIMEOUT
    }
    class Request
    {
        /*  1. Get Header Info 
         *  2. Send Request 
         * 
         * 
         */
        enum requestType { 
            GET,POST,PUT,DELETE
        }

        requestType _reqMethod;
        StreamReader _reqReader;
        string route = "";
        Dictionary<string, string> header; 
        
        public Request(StreamReader reader) //handle input from client
        {
            _reqReader = reader;
            //ReadHttpHeader(out var contentLength);
        }

        public responseType GetRequestContent()
        {
            string line;
            int contentLength = 0;
            int lineCount = 0;

            if (_reqReader.EndOfStream)
            {
                return responseType.ERR;
            }

            while (!string.IsNullOrWhiteSpace(line = _reqReader.ReadLine()))
            {
                if(lineCount == 0)
                {
                    string[] parts = line.Split(" ");
                    if (parts.Length < 3)
                        return responseType.ERR;
                    if (!Enum.TryParse(typeof(requestType), parts[0], out object reqMethod))
                        return responseType.ERR;
                    this._reqMethod = (requestType)reqMethod;
                    string reqPath = parts[1];
                    string reqVersion = parts[2];
                    lineCount++;
                }
                if (line.ToLower().StartsWith("content-length:"))
                {
                    contentLength = int.Parse(line.Substring(15).Trim());
                    //Console.WriteLine($"ContentLen : {contentLength}");
                    if (contentLength > 1024)
                        return responseType.ERR;
                    break;
                }
            }

            return ReadHttpBody(contentLength);
        }

        private responseType ReadHttpBody(int contentLength)
        {
            if (contentLength == 0)
                return responseType.ERR;

            StringBuilder contentStringBuilder = new StringBuilder(10000);
            char[] buffer = new char[1024];
            int bytesReadTotal = 0;
            while (bytesReadTotal < contentLength)
            {
                int bytesRead = _reqReader.Read(buffer, 0, 1024);
                bytesReadTotal += bytesRead;
                if (bytesRead == 0)
                {
                    return responseType.ERR;
                }
                contentStringBuilder.Append(buffer, 0, bytesRead);
            }
            Console.WriteLine(contentStringBuilder.ToString());
            /*
            string line;
            while (!string.IsNullOrWhiteSpace(line = _reqReader.ReadLine()))
            {
                int pos = line.IndexOf(":");
                string key = line.Substring(0, pos);
                string value = line.Substring(pos + 1, line.Length - pos - 1).TrimStart();
                Console.WriteLine($"{key} : {value}");
            }
            */
            //contentStringBuilder.ToString()
            return responseType.OK ;
        }

    }
}
