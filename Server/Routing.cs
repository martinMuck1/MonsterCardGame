using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            Handler handleObj = null;

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
                int contentLength = Convert.ToInt32(_req.Header["ContentLength"]);
                output = _req.ReadHttpBody(contentLength);
                if(reqPath == "tradings" && param != "") //special deserialisation, because same route but different json
                {
                    PostTrade obj = new PostTrade(AuthLevel.Login);
                    obj.Param = param;
                    obj.DeserializeMessageAlternative(output);
                    lock (Session._lockObject)
                    {
                        obj.Handle(_res, token);
                    }
                    return;
                }
                InitPostDic();
                handleObj = _methodDict[reqPath];
                handleObj.Param = param;
                handleObj.DeserializeMessage(output);
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
                handleObj = _methodDict[reqPath];
                handleObj.Param = param;
            }
            if (_req.ReqMethod == requestType.PUT)
            {
                InitPutDic();
                int contentLength = Convert.ToInt32(_req.Header["ContentLength"]);
                output = _req.ReadHttpBody(contentLength);
                handleObj = _methodDict[reqPath];
                handleObj.DeserializeMessage(output);
                handleObj.Param = param;
            }
            if (_req.ReqMethod == requestType.DELETE)
            {
                InitDeleteDic();
                handleObj = _methodDict[reqPath];
                handleObj.Param = param;
            }
            if(handleObj != null)
            {
                lock (Session._lockObject)
                {
                    handleObj.Handle(_res, token);
                }
            }
        }

        //mapping routes to handlers
        private void InitPostDic(){
            this._methodDict.Add("users", new UserHandle(AuthLevel.noLogin));
            this._methodDict.Add("sessions", new LoginHandle(AuthLevel.noLogin));
            this._methodDict.Add("packages", new PostCreatePackage(AuthLevel.Admin));
            this._methodDict.Add("transactions/packages", new PostAquirePackage(AuthLevel.Login));
            this._methodDict.Add("battles", new PostBattle(AuthLevel.Login));
            this._methodDict.Add("tradings", new PostTrade(AuthLevel.Login));
        }

        private void InitGetDic()
        {
            this._methodDict.Add("cards", new GetStack(AuthLevel.Login));
            this._methodDict.Add("deck", new GetDeck(AuthLevel.Login));
            this._methodDict.Add("users", new GetUserData(AuthLevel.Login));
            this._methodDict.Add("stats", new GetUserStats(AuthLevel.Login));
            this._methodDict.Add("score", new GetScoreboard(AuthLevel.Login));
            this._methodDict.Add("tradings", new GetTrades(AuthLevel.Login));
        }

        private void InitPutDic()
        {
            this._methodDict.Add("deck", new PutDeck(AuthLevel.Login));
            this._methodDict.Add("users", new PutUserData(AuthLevel.Login));
        }

        private void InitDeleteDic()
        {
            this._methodDict.Add("tradings", new DeleteTrade(AuthLevel.Login));
        }
    }
}
