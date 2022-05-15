// See https://aka.ms/new-console-template for more information


namespace LAN_Chat_App
{
    class Program
    {
        private const int port = 8888;
        static void Main(string[] args)
        {
            string deviceIP = IPHelper.GetMyIPAddress();
            ////List<string> ipAddressesOnNetwork = IPHelper.GetAllIPAddresses(deviceIP);
            IPHelper.GetAllIPAddresses(deviceIP);
            Server server = new();
            server.Create(deviceIP,port).Start().StartListen();
            Client client = new();
            while (true)
            {
                Console.WriteLine("Enter receiver ip:");
                if (Console.ReadLine() is string ip)
                    client.ConnectToServer(ip, port);
                if (client.IsConnected())
                {
                    Console.WriteLine("Enter you message:");
                    if (Console.ReadLine() is string message)
                    {
                        client.SendMessage(message);
                    }
                }
                else
                {
                    Console.WriteLine("Fialed connected...");
                }
            }
        }
    }
}