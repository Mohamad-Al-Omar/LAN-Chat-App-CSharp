using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace LAN_Chat_App
{
    internal class IPHelper
    {
        private static CountdownEvent? countDown;
        const bool resolveNames = true;
        static readonly List<string> ipAddresses = new();
        static string deviceIP = "";
        public static  string GetMyIPAddress()
        {
            using Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, 0);
             socket.Connect("8.8.8.8", 65530);
            if (socket.LocalEndPoint is IPEndPoint endPoint) 
            {
                Console.WriteLine("Your device ip is {0}", endPoint.Address.ToString());
                return endPoint.Address.ToString();
            }
                
            return "Cannot find device ip address";
        }

        public static List<string> GetAllIPAddresses(string userDeviceIP)
        {
            Console.WriteLine("Starting search for other devices ip address...");
            deviceIP = userDeviceIP;
            ipAddresses.Clear();
            countDown = new(1);
            Stopwatch sw = new();
            sw.Start();
            string ipBase = "192.168.0.";
            for (int i = 1; i < 255; i++)
            {
                string ip = ipBase + i.ToString();

                Ping p = new();
                p.PingCompleted += new(P_PingCompleted);
                countDown.AddCount();
                p.SendAsync(ip, 100, ip);
            }
            countDown.Signal();
            countDown.Wait();
            sw.Stop();
            Console.WriteLine("Finish search.");
            return ipAddresses;
        }

        private static void P_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            string? ip = e.UserState as string;
            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {

                if (resolveNames)
                {
                    string name;
                    try
                    {
                        IPHostEntry hostEntry = Dns.GetHostEntry(ip ?? "");
                        name = hostEntry.HostName;
                    }
                    catch (SocketException)
                    {
                        name = "Cannot find a device name ..!";
                    }
                    if (ip != "192.168.0.1")
                    {
                        string fullAddress = "";
                        if (ip == deviceIP)
                        {
                            fullAddress = ip + " ==> " + name + " it's your machine";
                        }
                        else
                        {
                            fullAddress = ip + " ==> " + name;
                        }
                        Console.WriteLine(fullAddress);
                        ipAddresses.Add(fullAddress);
                    }
                }
                else
                {
                    Console.WriteLine("{0} is up: ({1} ms)", ip, e.Reply.RoundtripTime);
                }
            }
            else if (e.Reply == null)
            {
                Console.WriteLine("Pinging {0} failed. (Null Reply object?)", ip);
            }
            countDown?.Signal();
        }
    }
}
