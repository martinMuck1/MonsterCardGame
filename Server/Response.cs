using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Server
{
    public enum responseType
    {
        OK = 200, NO_CONTENT = 204, ERR = 400, UNAUTHORIZED = 401, NOT_FOUND = 404
    }
    public class Response
    {
        public StreamWriter Writer { get; private set;}
        public responseType RespType { get; private set; }
        public string Message { get; private set; } = "";
        private int _contentLength;

        //private _contentType = "
        public Response(StreamWriter writer)
        {
            this.Writer = writer;
        }

        public void SendBadRequest(responseType resp)
        {
            this.RespType = resp;
            this._contentLength = 0;
            Send();
        }

        public void SendResponse(responseType rep, string message)
        {
            //this.Message = (string)JsonConvert.SerializeObject(message);
            this.Message = message;
            this.RespType = rep;
            this._contentLength = System.Text.ASCIIEncoding.UTF8.GetByteCount(message);
            Send();
        }

        private void Send()
        {
            //TODO: mit dictionary
            this.Writer.WriteLine($"HTTP/1.1 {(int)this.RespType}");
            this.Writer.WriteLine($"Content-Length: {this._contentLength}"); 
            this.Writer.WriteLine("Content-Type: application/json");
            this.Writer.WriteLine("Connection: Closed");
            this.Writer.WriteLine("");
            this.Writer.WriteLine(this.Message);
            if (this.Message == "")
            {
                Writer.Close();
                return;
            }

            Writer.Close();

        }
    }
}
