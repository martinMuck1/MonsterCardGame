using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MonsterCardGame.Server
{
    public enum AuthLevel
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
        }

        public void FindRoute()
        {
            string output = "";
            string reqPath = _req.Header["Path"];
            string token = "";
            if (_req.Header.ContainsKey("Authorization"))
                token = _req.Header["Authorization"];

            if (_req.ReqMethod == requestType.POST)
            {
                InitPostDic();
                int contentLength = Convert.ToInt32(_req.Header["ContentLength"]);
                output = _req.ReadHttpBody(contentLength);
                Handler handleObj = _methodDict[reqPath];
                handleObj.DeserializeMessage(output);
                handleObj.Handle(_res, token);
            }
            if (_req.ReqMethod == requestType.GET)
            {
                InitGetDic();
                Handler handleObj = _methodDict[reqPath];
                handleObj.Handle(_res, token);
            }
            /*
            if (reqPath.Contains("/") || reqPath.Contains("?"))
            {
                //further checking of path=> users/martin
            }
            */
        }

        //mapping routes to handlers
        private void InitPostDic(){
            this._methodDict.Add("users", new UserHandle(AuthLevel.noLogin));
            this._methodDict.Add("sessions", new LoginHandle(AuthLevel.noLogin));
            this._methodDict.Add("packages", new CreatePackage(AuthLevel.Admin));
            this._methodDict.Add("transactions/packages", new AquirePackage(AuthLevel.Login));
        }

        private void InitGetDic()
        {
            this._methodDict.Add("cards", new ShowCardsHandle(AuthLevel.noLogin));
        }


    }
}
