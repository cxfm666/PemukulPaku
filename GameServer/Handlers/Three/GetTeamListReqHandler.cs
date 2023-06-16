using Common.Resources.Proto;
using PemukulPaku.GameServer.MPModule;

namespace PemukulPaku.GameServer.Handlers.Three
{
    [PacketCmdId(CmdId.GetTeamListReq)]
    internal class GetTeamListReqHandler : IPacketHandler
    {
        public void Handle(Session session, Packet packet)
        {
            GetTeamListReq Data = packet.GetDecodedBody<GetTeamListReq>();
            GetTeamListRsp Rsp = new() { retcode = GetTeamListRsp.Retcode.Succ };
            Rsp.TeamDataLists.AddRange(Lobby.GetInstance().Teams.Values.Where(x => Data.StageIdLists.Contains(x.StageId)).Select(x => x.GetMpTeamData()));

            session.Send(Packet.FromProto(Rsp, CmdId.GetTeamListRsp));
        }
    }
}
