using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyTCPListener
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;

            try
            {
                var port = 1020;
                IPAddress localAddress = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddress, port);
                server.Start();

                byte[] bytes = new byte[256];
                string data = null;

                while (true)
                {
                    Console.WriteLine("Waiting for connection...");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    NetworkStream stream = client.GetStream();

                    int i;

                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.UTF8.GetString(bytes, 0, i);
                        Console.WriteLine(data);

                        byte[] message = Encoding.UTF8.GetBytes(data);

                        stream.Write(message, 0, message.Length);
                    }

                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }
            finally
            {
                server.Stop();
            }

            Console.WriteLine("Hit enter to continue...");
        }
    }
}
