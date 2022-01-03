using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonsterCardGame.Server
{
    
    class HTTPServer
    {
        private TcpListener listener;
        public static Random random = new Random(0);

        public HTTPServer(int portNum)  //start server at 127.0.0.1 at Port 8000
        {
            listener = new TcpListener(IPAddress.Loopback, portNum);
            
        }
        public async Task StartServerAsync()
        {
            listener.Start(5);
            //Console.CancelKeyPress += (sender, e) => Environment.Exit(0);
            //HTTPServer.SessionID = Guid.NewGuid().ToString();
            while (true)
            {
                try
                {   
                    //listener.Start();
                    Console.WriteLine("waiting for client ....");
                    var client = await listener.AcceptTcpClientAsync();
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
            listener.Stop();
        }

        private static void StartClientHandling(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            using var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            using var reader = new StreamReader(client.GetStream());
            Console.WriteLine("Got Request from Client");
            responseType respType = responseType.ERR;
            Request req = new Request(reader, out respType);
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
            res.SendResponse(responseType.OK,"{message: something}");
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
