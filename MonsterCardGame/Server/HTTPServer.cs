using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MonsterCardGame;


namespace MonsterCardGame.Server
{
    
    class HTTPServer
    {
        private TcpListener _listener;
        public static Random random = new Random(0);    //the seed 0 is on purpose => create same packages always for integration tests
        private bool _loopVar = true;

        public HTTPServer(int portNum)  //start server at 127.0.0.1 at Port 8000
        {
            _listener = new TcpListener(IPAddress.Loopback, portNum);
            
        }
        public void StartServer()
        {
            _listener.Start(5);
            //Console.CancelKeyPress += (sender, e) => this._loopVar = false;
            while (_loopVar)
            {
                try
                {   
                    Console.WriteLine("waiting for client ....");
                    var client =  _listener.AcceptTcpClient();
                    Console.WriteLine("new client connected");
                    Thread clientThread = new Thread(new ParameterizedThreadStart(StartClientHandling));
                    clientThread.Start(client);
                    //Task task1 = Task.Factory.StartNew(() => Test(client));
                }
                catch (Exception exc)
                {
                    Console.WriteLine("error occurred: " + exc.Message);
                }
            }
            Database.Database.getInstance().CloseConnection();
            _listener.Stop();
        }

        private static void StartClientHandling(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            using var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            using var reader = new StreamReader(client.GetStream());
            Console.WriteLine("Got Request from Client");
            Request req = new Request(reader);
            responseType respType =req.GetRequestHeader();
            Response res = new Response(writer);
            if (respType == responseType.OK)
            {
                Routing route = new Routing(req, res);
                route.FindRoute();
            }
            else
            {
                res.SendBadRequest(respType);
            }
        }

        /*
        private static void Test(Object obj)
        {
            Console.WriteLine("special loop");
            Thread.Sleep(30000);
            Console.WriteLine("special loop");
            TcpClient client = (TcpClient)obj;
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            using var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            Response res = new Response(writer);
            res.SendResponse(responseType.OK,"{message: something special}");
            for (int i = 0; i < 1000; i++)
            {
                if(i%100 == 0)
                {
                Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId}  {i}");
                }
            }
        }
        private static void Test2(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            using var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            Response res = new Response(writer);
            res.SendResponse(responseType.OK, "{message: something}");
            for (int i = 0; i < 1000; i++)
            {
                if (i % 100 == 0)
                {
                    Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId}  {i}");
                }
            }
        }
        */
    }
}
