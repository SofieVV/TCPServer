using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyTCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter server name: ");
            string server = Console.ReadLine();

            Connect(server);
        }

        static void Connect(string server)
        {
            try
            {
                var port = 1020;
                var client = new TcpClient(server, port);

                NetworkStream stream = client.GetStream();

                while (true)
                {
                    string message = Console.ReadLine();
                    if (message == "quit")
                        break;

                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                    data = new Byte[256];

                    var responseData = string.Empty;

                    var bytes = stream.Read(data, 0, data.Length);
                    responseData = Encoding.UTF8.GetString(data, 0, bytes);
                }

                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"ArgumentNullException: {e}");
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }

            Console.WriteLine("Press Enter to continue...");
        }
    }
}
