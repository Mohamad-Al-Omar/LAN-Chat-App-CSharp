using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LAN_Chat_App
{
    internal class Client
    {
        private TcpClient? serverSocket;
        private bool isConnected = false;

        public Client ConnectToServer(string ip, int port)
        {
            try 
            {
                serverSocket = new(ip, port);
                isConnected = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to server at {0}:{1} with error message {2}", ip, port,ex.Message);
                isConnected = false;
            }
            return this;
        }

        public Client SendMessage(string message)
        {
            if (isConnected && serverSocket != null)
            {
                NetworkStream networkStream = serverSocket.GetStream();
                StreamReader streamReader = new(networkStream);
                StreamWriter streamWriter = new(networkStream);
                try
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Flush();
                }
                catch
                {
                    Console.WriteLine("Exception reading from Server");
                }
                finally
                {
                    // tidy up
                    networkStream.Close();
                }
            }
            else
            {
                Console.WriteLine("Please connect to receier first.");
            }
            return this;
        }

        public Client Disconnect()
        {
            serverSocket?.Close();
            return this;
        }

        public bool IsConnected() 
        {
            return isConnected;
        }
    }
}
