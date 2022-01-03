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
            string output= "", token = "", param = "";
            string reqPath = _req.Header["Path"];

            if (_req.Header.ContainsKey("Authorization"))
                token = _req.Header["Authorization"];

            if(reqPath.Contains("/") && reqPath != "transactions/packages")
            {
                string[] wholePath = reqPath.Split("/");
                reqPath = wholePath[0];
                param = wholePath[1];
            }

            if (_req.ReqMethod == requestType.POST)
            {
                InitPostDic();
                int contentLength = Convert.ToInt32(_req.Header["ContentLength"]);
                output = _req.ReadHttpBody(contentLength);
                Handler handleObj = _methodDict[reqPath];
                handleObj.DeserializeMessage(output);
                handleObj.Param = param;
                handleObj.Handle(_res, token);
            }
            if (_req.ReqMethod == requestType.GET )
            {
                InitGetDic();
                if (reqPath.Contains("?"))
                {
                    string[] wholePath = reqPath.Split("?");
                    reqPath = wholePath[0];
                    param = wholePath[1];
                }
                Handler handleObj = _methodDict[reqPath];
                handleObj.Param = param;
                handleObj.Handle(_res, token);
            }
            if (_req.ReqMethod == requestType.PUT)
            {
                InitPutDic();
                int contentLength = Convert.ToInt32(_req.Header["ContentLength"]);
                output = _req.ReadHttpBody(contentLength);
                Handler handleObj = _methodDict[reqPath];
                handleObj.DeserializeMessage(output);
                handleObj.Param = param;
                handleObj.Handle(_res, token);
            }
            if (_req.ReqMethod == requestType.DELETE)
            {
                InitDeleteDic();
                Handler handleObj = _methodDict[reqPath];
                handleObj.Param = param;
                handleObj.Handle(_res, token);
            }
        }

        //mapping routes to handlers
        private void InitPostDic(){
            this._methodDict.Add("users", new UserHandle(AuthLevel.noLogin));
            this._methodDict.Add("sessions", new LoginHandle(AuthLevel.noLogin));
            this._methodDict.Add("packages", new CreatePackage(AuthLevel.Admin));
            this._methodDict.Add("transactions/packages", new AquirePackage(AuthLevel.Login));
            this._methodDict.Add("battles", new PostBattle(AuthLevel.Login));
            this._methodDict.Add("trades", new PostTrade(AuthLevel.Login));
        }

        private void InitGetDic()
        {
            this._methodDict.Add("cards", new GetStack(AuthLevel.Login));
            this._methodDict.Add("deck", new GetDeck(AuthLevel.Login));
            this._methodDict.Add("users", new GetUserData(AuthLevel.Login));
            this._methodDict.Add("stats", new GetUserStats(AuthLevel.Login));
            this._methodDict.Add("score", new GetScoreboard(AuthLevel.Login));
            this._methodDict.Add("trades", new GetTrades(AuthLevel.Login));
        }

        private void InitPutDic()
        {
            this._methodDict.Add("deck", new PutDeck(AuthLevel.Login));
            this._methodDict.Add("users", new PutUserData(AuthLevel.Login));
        }

        private void InitDeleteDic()
        {
            this._methodDict.Add("trades", new DeleteTrade(AuthLevel.Login));
        }
    }
}
