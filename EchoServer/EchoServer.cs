using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace EchoServer
{
    class EchoServer
    {
        static void Main(string[] args)
        {
            //Opret server
            IPAddress ip = IPAddress.Parse("192.168.24.205");
            //Opret adresse eller port
            TcpListener serverSocket = new TcpListener(ip, 7777);
            //starter server
            serverSocket.Start();
            Console.WriteLine("Server Started");

            //loop åbner for mulighed for at connecte flere clients
            do
            {
                Task.Run(() =>
                {
                    //Venter på connection før den aktiverer
                    TcpClient connectionSocket = serverSocket.AcceptTcpClient();
                    Console.WriteLine("Server activated & Connected");
                    //kalder metoden DoClient
                    DoClient(connectionSocket);

                   });

            } while (true);

            serverSocket.Stop();
            Console.WriteLine("Server stopped & Disconnected");
        }

        public static void DoClient(TcpClient socket)
        {
            Stream ns = socket.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true;

            string message = sr.ReadLine();
            string answer = "";

            while (message != null && message != "")
            {
                Console.WriteLine("Client: " + message);
                answer = message.ToUpper();
                sw.WriteLine(answer);
                message = sr.ReadLine();
            }

            ns.Close();
            socket.Close();

        }
    }
}
