using Common.Resources.Proto;
using PemukulPaku.GameServer.MPModule;

namespace PemukulPaku.GameServer.Handlers.One
{
    [PacketCmdId(CmdId.GetTeamBriefInfoReq)]
    internal class GetTeamBriefInfoReqHandler : IPacketHandler
    {
        public void Handle(Session session, Packet packet)
        {
            GetTeamBriefInfoReq Data = packet.GetDecodedBody<GetTeamBriefInfoReq>();
            Team? team = Lobby.GetInstance().Teams.Values.FirstOrDefault(x => x.LeaderUid == Data.TeamId);
            if (team is null)
                return;

            GetTeamBriefInfoRsp Rsp = new()
            {
                retcode = GetTeamBriefInfoRsp.Retcode.Succ,
                TeamId = Data.TeamId,
                IsFriendInvitation = Data.IsFriendInvitation,
                StageId = team.StageId,
                Status = MpTeamStatus.TeamStatusInLobby
            };

            session.Send(Packet.FromProto(Rsp, CmdId.GetTeamBriefInfoRsp));
        }
    }
}
