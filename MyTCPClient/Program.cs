using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyTCPClient
{
    class Program
    {
        private static int port = 1020;

        static void Main(string[] args)
        {
            Console.Write("Please enter server name: ");
            string server = Console.ReadLine();

            Connect(server);
        }

        public static void Connect(string server)
        {
            try
            {
                var client = new TcpClient(server, port);

                NetworkStream stream = client.GetStream();

                Thread thread = new Thread(Read);
                thread.Start(client);

                while (true)
                {
                    string message = Console.ReadLine();

                    if (message == "quit")
                    {
                        client.Close();
                        break;
                    }

                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"ArgumentNullException: {e}");
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }
        }

        public static void Read(object obj)
        {
            try
            {
                var client = (TcpClient)obj;

                while (true)
                {
                    NetworkStream stream = client.GetStream();
                    var data = new Byte[256];
                    int counter = stream.Read(data, 0, data.Length);
                    string message = Encoding.UTF8.GetString(data).TrimEnd('\0');

                    if (counter == 0)
                        break;
                    else
                        Console.WriteLine(message);
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Disconnected.");
            }
        }
    }
}
