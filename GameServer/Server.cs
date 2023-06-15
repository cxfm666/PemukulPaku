using System.Net.Sockets;
using System.Net;
using Common;
using Common.Utils;

namespace PemukulPaku.GameServer
{
    public class Server
    {
        public static readonly Logger c = new("TCP", ConsoleColor.Blue);
        public readonly Dictionary<string, Session> Sessions = new();
        private static Server? Instance;

        public static Server GetInstance()
        {
            return Instance ??= new Server();
        }

        public Server()
        {
            Task.Run(Start);
        }

        public void Start()
        {
            TcpListener Listener = new(IPAddress.Parse("0.0.0.0"), (int)Global.config.Gameserver.Port);

            while (true)
            {
                try
                {
                    Listener.Start();
                    c.Log($"TCP 服务器已在 {Global.config.Gameserver.Port} 端口上启动！");

                    while (true)
                    {
                        TcpClient Client = Listener.AcceptTcpClient();
                        string Id = Client.Client.RemoteEndPoint!.ToString()!;

                        c.Warn($"{Id} 已连接");
                        Sessions.Add(Id, new Session(Id, Client));
                        LogClients();
                    }
                }
                catch (Exception ex)
                {
                    c.Error("TCP 服务器错误: " + ex.Message);
                    Thread.Sleep(3000);
                }
            }
        }

        public void LogClients()
        {
            c.Log($"已连接的客户端: {Sessions.Count}");
        }
    }
}
