using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MonsterCardGame.Server
{
    public enum authLevel
    {
        noLogin = 1, Login = 2, Admin =3,
    }
    public class Routing
    {
        private Request _req;
        private Response _res;
        private Dictionary<string,Handler> _methodDict = new();

        public Routing(Request req, Response res)
        {
            this._req = req;
            this._res = res;
            //initDic();
        }

        public void FindRoute()
        {
            string output;
            string reqPath = _req.Header["Path"];
            string token = "";
            if (_req.Header.ContainsKey("Authorization"))
                token = _req.Header["Authorization"];

            if (_req.ReqMethod == requestType.POST)
            {
                int contentLength = Convert.ToInt32(_req.Header["ContentLength"]);
                output = _req.ReadHttpBody(contentLength);
                initDic(output);
                //Console.WriteLine(JsonConvert.SerializeObject(tmpJSON));
            }

            if (reqPath.Contains("/") || reqPath.Contains("?"))
            {
                //further checking of path=> users/martin
            }
            Handler handleObj = _methodDict[reqPath];
            handleObj.Handle(_res, token);
        }

        private void initDic(string message){
            this._methodDict.Add("users", new UserHandle(message,authLevel.noLogin));
            //this._methodDict.Add("sessions", new UserHandle(message));
        }


    }
}
