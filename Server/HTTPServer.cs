using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Server
{
    
    class HTTPServer
    {
        private TcpListener listener;
        public static Random random = new Random();

        public HTTPServer(int portNum)  //start server at 127.0.0.1 at Port 8000
        {
            listener = new TcpListener(IPAddress.Loopback, portNum);
            
        }
        public async Task StartServerAsync() //TODO multithreaded multiple clients
        {
            listener.Start(5);
            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);
            //HTTPServer.SessionID = Guid.NewGuid().ToString();
            while (true)
            {
                try
                {
                    var client = await listener.AcceptTcpClientAsync();
                    using var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
                    using var reader = new StreamReader(client.GetStream());
                    Console.WriteLine("Got Request from Client");
                    responseType respType = responseType.ERR;
                    Request req = new Request(reader, out respType);
                    Response res = new Response(writer);
                    if(respType == responseType.OK)
                    {
                        Routing route = new Routing(req, res);
                        route.FindRoute();
                    }
                    else
                    {
                        res.SendBadRequest(respType);
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine("error occurred: " + exc.Message);
                }
            }
            listener.Stop();
        }
    }
}
