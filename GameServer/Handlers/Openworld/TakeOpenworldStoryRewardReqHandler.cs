using Common.Database;
using Common.Resources.Proto;
using Common.Utils.ExcelReader;

namespace PemukulPaku.GameServer.Handlers.Openworld
{
    [PacketCmdId(CmdId.TakeOpenworldStoryRewardReq)]
    internal class TakeOpenworldStoryRewardReqHandler : IPacketHandler
    {
        public void Handle(Session session, Packet packet)
        {
            TakeOpenworldStoryRewardReq Data = packet.GetDecodedBody<TakeOpenworldStoryRewardReq>();
            UserScheme.OpenWorldStoryScheme? openWorldStory = session.Player.User.OpenWorldStory.FirstOrDefault(x => x.StoryId == Data.StoryId);
            if (openWorldStory is not null)
                openWorldStory.IsDone = true;

            OpenWorldStoryDataExcel? storyData = OpenWorldStoryData.GetInstance().FromId((int)Data.StoryId);
            if (storyData is not null)
            {
                OpenWorldScheme? ow = session.Player.OpenWorlds.Where(x => x.MapId == storyData.StoryMap).FirstOrDefault();
                if (ow is not null && OpenWorldStoryData.GetInstance().All.FirstOrDefault(story => story.PreStory.Contains((int)Data.StoryId) && story.StoryMap == storyData.StoryMap)?.Cycle > storyData.Cycle)
                {
                    ow.Cycle = OpenWorldCycleData.GetInstance().GetNextCycle((uint)storyData.StoryMap, (uint)storyData.Cycle);
                    ow.HasTakeFinishRewardCycle = OpenWorldCycleData.GetInstance().GetNextCycle((uint)storyData.StoryMap, (uint)storyData.Cycle);
                }
            }

            session.Send(Packet.FromProto(new TakeOpenworldStoryRewardRsp() { retcode = TakeOpenworldStoryRewardRsp.Retcode.Succ, StoryId = Data.StoryId }, CmdId.TakeOpenworldStoryRewardRsp));
            session.ProcessPacket(Packet.FromProto(new GetOpenworldStoryReq() { }, CmdId.GetOpenworldStoryReq));
        }
    }
}
