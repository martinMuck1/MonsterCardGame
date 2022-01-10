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
    public class PostTransaction :Handler
    {
        struct formatTransaction
        {
            public string recipient;
            public int amount;
        }

        private formatTransaction _reqTransaction;

        public PostTransaction( AuthLevel level) :base(level)
        {
            
        }

        public override void DeserializeMessage(string message)
        {
            _reqTransaction = JsonConvert.DeserializeObject<formatTransaction>(message);
        }

        public override void Handle(Response res,string token)
        {
            string username = "";   //should be admin then
            if (!CheckAuth(res, token, ref username))
                return;

            UserDao userdao = new UserDao();
            List<UserModel> userList = userdao.GetAllUsers();
            if(!userList.Any(user => user.Username == _reqTransaction.recipient))       //recipient from request is not user
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"Recipient does not exist\"}");
                Console.WriteLine("Recipient of Transaction doesnt exist");
            }
            if(userdao.PayWithCoins(_reqTransaction.recipient, -_reqTransaction.amount) != 0)
                res.SendResponse(responseType.ERR, "{\"message\": \"Could not update Coins from user\"}");

            TransactionModel model = new TransactionModel(_reqTransaction.recipient, _reqTransaction.amount, null);
            if (userdao.InsertTransaction(model) != 0)
                res.SendResponse(responseType.ERR, "{\"message\": \"Could not set Transaction\"}");

            res.SendResponse(responseType.OK, $"{{\"message\":\"Updated Coins for user {_reqTransaction.recipient} successfully\"}}");
        }
    }
}
