using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MonsterCardGame.Database;
using Newtonsoft.Json.Linq;

namespace MonsterCardGame.Server
{
    public class GetTransactions :Handler
    {
        private Dictionary<int, string> _user = new Dictionary<int, string>();
        public GetTransactions( AuthLevel level) :base(level)
        {
            
        }

        public override void DeserializeMessage(string message)
        {

        }

        public override void Handle(Response res,string token)
        {
            string username = "";
            if (!CheckAuth(res, token, ref username))
                return;

            UserDao userdao = new UserDao();
            List<TransactionModel> modelList = userdao.GetTransactions(DBHelper.ConvertNameToID(username));
            JArray array = new JArray();
            foreach (var transaction in modelList)
            {
                JObject obj = new JObject();
                obj["transactionID"] = transaction.TID;
                obj["package"] = transaction.PackageID;
                obj["amount"] = transaction.Amount;
                obj["time"] = transaction.Timestamp;
                obj["user"] = username;
                array.Add(obj);
            }
            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(array));
            Console.WriteLine("Sent Transactions");
        }

    }
}
