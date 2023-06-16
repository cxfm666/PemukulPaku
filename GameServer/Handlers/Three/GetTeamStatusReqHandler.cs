using Common.Resources.Proto;
using PemukulPaku.GameServer.MPModule;

namespace PemukulPaku.GameServer.Handlers.Three
{
    [PacketCmdId(CmdId.GetTeamStatusReq)]
    internal class GetTeamStatusReqHandler : IPacketHandler
    {
        public void Handle(Session session, Packet packet)
        {
            GetTeamStatusReq Data = packet.GetDecodedBody<GetTeamStatusReq>();
            GetTeamStatusRsp Rsp = new() { retcode = GetTeamStatusRsp.Retcode.Succ };
            Rsp.TeamDataLists.AddRange(Lobby.GetInstance().Teams.Values.Where(x => Data.TeamIdLists.Contains(x.LeaderUid)).Select(x => x.GetMpTeamData()));

            session.Send(Packet.FromProto(Rsp, CmdId.GetTeamStatusRsp));
        }
    }
}
