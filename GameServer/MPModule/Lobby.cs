using System;
using Common;
using Common.Resources.Proto;

namespace PemukulPaku.GameServer.MPModule
{
    public class Lobby
    {
        private static Lobby? Instance;
        public readonly Dictionary<uint, Team> Teams = new();

        public static Lobby GetInstance()
        {
            return Instance ??= new Lobby();
        }

        public Team CreateTeam(Team team)
        {
            if (Teams.GetValueOrDefault(team.LeaderUid) is null)
                Teams.Add(team.LeaderUid, team);

            SyncTeam(team.LeaderUid);
            return team;
        }

        public void SyncTeam(uint teamId)
        {
            Teams.TryGetValue(teamId, out Team? team);
            if (team is null)
                return;

            MpTeamSyncNotify teamSyncNotify = new()
            {
                TeamData = team.GetMpTeamData()
            };

            foreach (Session? session in team.Members.Where(x => x.Session is not null).Select(x => x.Session))
            {
                session?.Send(Packet.FromProto(teamSyncNotify, CmdId.MpTeamSyncNotify));
            }
        }
    }

    public class Team
    {
        public uint StageId { get; set; }
        public uint MinLevel;
        public uint LeaderUid;
        public LobbyEnterType LobbyEnterType;
        public LobbyStatus LobbyStatus = LobbyStatus.LobbyPreparing;
        public List<TeamMember> Members;
        public string Name;

        public Team(uint stageId, uint minLevel, LobbyEnterType lobbyEnterType, in Session leader, string name)
        {
            StageId = stageId;
            MinLevel = minLevel;
            LobbyEnterType = lobbyEnterType;
            Members = new List<TeamMember> { new(leader), new(null, 2), new(null, 3) };
            LeaderUid = leader.Player.User.Uid;
            Name = name;
        }

        public void Join(Session session)
        {
            if (Members[1].Session is null)
                Members[1].Session = session;
            else if (Members[2].Session is null)
                Members[2].Session = session;

            Lobby.GetInstance().SyncTeam(LeaderUid);
        }

        public MpTeamData GetMpTeamData()
        {
            MpTeamData teamData = new()
            {
                LeaderUid = LeaderUid,
                TeamId = LeaderUid,
                LobbyEnterType = LobbyEnterType,
                LobbyStatus = LobbyStatus,
                MinLevel = MinLevel,
                MaxLevel = 0,
                StageId = StageId,
                TeamName = Name,
                Status = MpTeamStatus.TeamStatusInLobby
            };
            teamData.MemberLists.AddRange(Members.Select(member =>
            {
                if (member.Session is null)
                    return new MpTeamMember() { Index = member.Index, DressId = 0 };

                return new MpTeamMember()
                {
                    Index = member.Index,
                    Uid = member.Session.Player.User.Uid,
                    MpExp = 0,
                    Stamina = (uint)member.Session.Player.User.Stamina,
                    HeadAvatarId = member.Session.Player.GetDetailData().LeaderAvatar.AvatarId,
                    DressId = member.Session.Player.GetDetailData().LeaderAvatar.DressId,
                    PunishEndTime = 0,
                    MemberInfo = new()
                    {
                        Card = member.Session.Player.GetCardData(),
                        Detail = member.Session.Player.GetDetailData()
                    },
                    Status = member.Status,
                    ClientStatus = member.ClientStatus,
                    AvatarTrialId = 0,
                    IsWild = true,
                    RegionName = Global.config.Gameserver.RegionName,
                    FrameId = member.Session.Player.GetDetailData().FrameId,
                    CustomHeadId = member.Session.Player.GetDetailData().CustomHeadId,
                    NewbieId = 0
                };
            }));

            return teamData;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class TeamMember
    {
        public Session? Session;
        public uint Index;
        public LobbyClientStatus ClientStatus { get; set; } = LobbyClientStatus.LobbyClientNone;
        public LobbyMemberStatus Status { get; set; } = LobbyMemberStatus.LobbyMemberReady;

        public TeamMember(Session? session = null, uint index = 1)
        {
            Session = session;
            Index = index;
        }
    }
}
