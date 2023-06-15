using Common;
using System.Net.NetworkInformation;
using PemukulPaku.GameServer;
using Common.Database;
using PemukulPaku.GameServer.Game;
using PemukulPaku.GameServer.Commands;
using MongoDB.Bson;
using Common.Resources.Proto;
using Common.Utils.ExcelReader;

namespace PemukulPaku
{
    class Program
    {
        public static void Main()
        {
#if DEBUG
            Global.config.VerboseLevel = VerboseLevel.Debug;
#endif
            Global.c.Log("启动服务器中...");
            CommandFactory.LoadCommandHandlers();
            PacketFactory.LoadPacketHandlers();
            new Thread(HttpServer.Program.Main).Start();
            _ = Server.GetInstance();
            ReadLine.GetInstance().Start();
        }
    }
}