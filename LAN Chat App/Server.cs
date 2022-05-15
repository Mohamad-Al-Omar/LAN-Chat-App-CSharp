using System.Net;
using System.Net.Sockets;

namespace LAN_Chat_App
{
    class Server
    {
        private TcpListener serverSocket;
        private TcpClient clientSocket;
        private int port;
        private string ipAddress;
        private Thread thread;
        private bool isStarted = false;

        //public  Server()
        //{
        //}

        public Server Create(string ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            this.serverSocket = new(IPAddress.Parse(ipAddress),this.port);
            Console.WriteLine("Server created successfully.");
            return this;
        }

        public Server Start()
        {
            try
            {
                if (this.serverSocket != null)
                {
                    this.serverSocket.Start();
                    Console.WriteLine("Server Started...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return this;
        }

        public Server StartListen()
        {
            this.isStarted = true;
            Thread thread = new(new ThreadStart(ServerListen));
            thread.Start();
            Console.WriteLine("Server in listening mode now..");
            return this;
        }

        private void ServerListen()
        {
            while (isStarted)
            {
                try
                {
                    clientSocket = serverSocket.AcceptTcpClient();
                    Byte[] bytes = new Byte[256];
                    String data = "";
                    NetworkStream stream = clientSocket.GetStream();
                    int i;

                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        //stream.Write(msg, 0, msg.Length);
                        //Console.WriteLine("Sent: {0}", data);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                
            } 
        }

    }
}
