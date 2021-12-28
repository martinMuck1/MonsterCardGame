using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Server
{         
    public enum requestType { 
            GET,POST,PUT,DELETE
    }
    public class Request       // handling request => functions for reading header and body
    {
        public StreamReader ReqReader { get; private set; }
        public Dictionary<string, string> Header { get; private set; } = new(); 
        public requestType ReqMethod { get; private set; }

        public Request(StreamReader reader, out responseType status ) //handle input from client
        {   //TODO: status gehört hier raus
            ReqReader = reader;
            status = GetRequestHeader();
        }

        private responseType GetRequestHeader()
        {
            string line;
            int contentLength = 0;
            int lineCount = 0;

            if (ReqReader.EndOfStream)
                return responseType.ERR;

            while (!string.IsNullOrWhiteSpace(line = ReqReader.ReadLine()))
            {
                if(lineCount == 0)
                {
                    string[] parts = line.Split(" ");
                    if (parts.Length != 3)
                        return responseType.ERR;
                    
                    if (!Enum.TryParse(typeof(requestType), parts[0], out object reqMethod))
                        return responseType.ERR;

                    this.ReqMethod = (requestType)reqMethod;
                    parts[1] = parts[1].Remove(0, 1);   //get rid of /
                    Header.Add("Path", parts[1]);
                    Header.Add("Version", parts[2]);
                    lineCount++;
                }
                if (line.ToLower().StartsWith("authorization:"))
                {
                    string tmp = line.Substring(15).Trim();
                    string[] val = tmp.Split(" "); 
                    if(val.Length != 2 || val[0] != "Basic")
                        return responseType.UNAUTHORIZED;

                    Header.Add("Authorization", val[1].Trim());
                }
                if (line.ToLower().StartsWith("content-length:"))
                {
                    contentLength = int.Parse(line.Substring(15).Trim());
                    if (contentLength > 1024 || contentLength < 0)
                        return responseType.ERR;

                    Header.Add("ContentLength", contentLength.ToString());
                    break;
                }
            }
            return responseType.OK;
        }

        public string ReadHttpBody(int contentLength)
        {
            StringBuilder contentStringBuilder = new StringBuilder(10000);
            char[] buffer = new char[1024];
            int bytesReadTotal = 0;
            while (bytesReadTotal < contentLength)
            {
                int bytesRead = ReqReader.Read(buffer, 0, 1024);
                bytesReadTotal += bytesRead;
                if (bytesRead == 0)
                {
                    return "";
                }
                contentStringBuilder.Append(buffer, 0, bytesRead);
            }
            return contentStringBuilder.ToString();
        }

    }
}
