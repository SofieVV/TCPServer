using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyTCPListener
{
    class Program
    {
        private static TcpListener server = null;
        private static List<TcpClient> clientList = new List<TcpClient>();
        private static byte[] bytes = new byte[256];
        private static string data = null;
        private static int port = 1020;
        private static IPAddress localAddress = IPAddress.Parse("127.0.0.1");

        static void Main(string[] args)
        {
            try
            {
                server = new TcpListener(localAddress, port);
                server.Start();
                Console.WriteLine("Waiting for connection...");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    clientList.Add(client);
                    Thread thread = new Thread(Listener);
                    thread.Start(client);
                    
                    Console.WriteLine("Client Connected!");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Client Disconnected.");
            }

            Console.WriteLine("Hit enter to continue...");
        }

        public static void Listener(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();

            try
            {
                while (true)
                {
                    int counter = stream.Read(bytes, 0, bytes.Length);
                    if (counter == 0)
                    {
                        Console.WriteLine("Client Disconnected.");
                        clientList.Remove(client);
                        break;
                    }

                    data = Encoding.UTF8.GetString(bytes, 0, counter).TrimEnd('\0');

                    BroadCast(data, client);
                    Console.WriteLine(data);
                }
        }
            catch (Exception)
            {
                clientList.Remove(client);
                Console.WriteLine("Client Disconnected.");
            }
}
        public static void BroadCast(string data, TcpClient currentClient)
        {
            string message;
            
                foreach (TcpClient client in clientList)
                {
                    NetworkStream stream = client.GetStream();
                    if (client != currentClient)
                    {
                        message = $"Friend: {data}";
                        byte[] buffer = Encoding.UTF8.GetBytes(message);
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }
    }
}
