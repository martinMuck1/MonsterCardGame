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
        private Dictionary<string,object> _methodDict = new();

        public Routing(Request req, Response res)
        {
            this._req = req;
            this._res = res;
            //initDic();
        }

        public void FindRoute()
        {
            string output;
            string path = _req.Header["Path"];

            if (_req.ReqMethod == requestType.POST)
            {
                int contentLength = Convert.ToInt32(_req.Header["ContentLength"]);
                output = _req.ReadHttpBody(contentLength);
                initDic(output);
                //Console.WriteLine(JsonConvert.SerializeObject(tmpJSON));
            }

            if (path.Contains("/") || path.Contains("?"))
            {
                //further checking of path=> users/martin
            }

            Handler handleObj = (Handler)_methodDict[path];
            handleObj.Handle();
            _res.SendResponse(responseType.OK, "");
        }

        private T GetJSON<T>(string output){

            return JsonConvert.DeserializeObject<T>(output);
        }

        private void initDic(string message){
            this._methodDict.Add("users", new UserHandle(message));
            this._methodDict.Add("sessions", new UserHandle(message));
        }


    }
}
