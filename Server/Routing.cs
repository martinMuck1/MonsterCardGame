using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MonsterCardGame.Server
{
    class Routing
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
            Handler handleObj = _methodDict["users"];
            _res.SendResponse( handleObj.Handle(),"");
        }

        private void initDic(string message){
            this._methodDict.Add("users", new UserHandle(message));
            this._methodDict.Add("sessions", new UserHandle(message));
        }


    }
}
