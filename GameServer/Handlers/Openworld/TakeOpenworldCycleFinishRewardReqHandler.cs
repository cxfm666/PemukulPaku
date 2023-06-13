using Common.Resources.Proto;

namespace PemukulPaku.GameServer.Handlers.Openworld
{
    [PacketCmdId(CmdId.TakeOpenworldCycleFinishRewardReq)]
    internal class TakeOpenworldCycleFinishRewardReqHandler : IPacketHandler
    {
        public void Handle(Session session, Packet packet)
        {
            TakeOpenworldCycleFinishRewardReq Data = packet.GetDecodedBody<TakeOpenworldCycleFinishRewardReq>();

            session.Send(Packet.FromProto(new TakeOpenworldCycleFinishRewardRsp()
            {
                retcode = TakeOpenworldCycleFinishRewardRsp.Retcode.Succ,
                MapId = Data.MapId,
                Cycle = Data.Cycle
            }, CmdId.TakeOpenworldCycleFinishRewardRsp));
        }
    }
}
