using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using BossLeaderboards.Source;
using BossLeaderboards.Source.Common.Player;

namespace BossLeaderboards.UI
{
    internal partial class LeaderboardSystem : ModSystem
    {
        private const int PacketTimerMax = 30;
        private int _packetTimer = PacketTimerMax;
        private const int StallTimerMax = 5 * 60;
        private int _stallTimer = 0;
        private bool _oldAnyActiveBossNPC = false;
        private List<Player> _trackedPlayers;

        public override void PreUpdateEntities()
        {
            if (FightJustBegan())
            {
                if (Main.netMode != NetmodeID.Server) {
                    leaderboard.RemoveAllData();
                    leaderboard.AddStatHeadings();
                }

                _trackedPlayers = Utilities.GetActivePlayers(Main.player);
                foreach (Player player in _trackedPlayers)
                {
                    LeaderboardsPlayer leaderboardsPlayer = player.GetModPlayer<LeaderboardsPlayer>();
                    leaderboardsPlayer.contribution = new Contribution();
                    if (Main.netMode != NetmodeID.Server) {
                        leaderboard.AddPlayer(player.whoAmI, leaderboardsPlayer.contribution);
                    }
                }
            }

            if ((Main.CurrentFrameFlags.AnyActiveBossNPC || --_stallTimer > 0) && --_packetTimer == 0)
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ContributionToServer();
                else if (Main.netMode == NetmodeID.Server)
                    LeaderboardToClients();

                _packetTimer = PacketTimerMax;
            }

            if (FightJustEnded())
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ContributionToServer();

                _stallTimer = StallTimerMax;
            }

            _oldAnyActiveBossNPC = Main.CurrentFrameFlags.AnyActiveBossNPC;
        }

        private bool FightJustBegan()
            => Main.CurrentFrameFlags.AnyActiveBossNPC && !_oldAnyActiveBossNPC;

        private bool FightJustEnded()
            => !Main.CurrentFrameFlags.AnyActiveBossNPC && _oldAnyActiveBossNPC;

        private void ContributionToServer()
        {
            Contribution contribution = Main.LocalPlayer.GetModPlayer<LeaderboardsPlayer>().contribution;
            ModPacket packet = Mod.GetPacket();
            foreach (string statName in Contribution.StatNames)
                packet.Write((long)contribution.GetStat(statName));
            packet.Send();
        }

        private void LeaderboardToClients()
        {
            List<Player> activeTracked = _trackedPlayers.Where(player => player.active).ToList();
            ModPacket packet = Mod.GetPacket();
            packet.Write(activeTracked.Count);
            foreach (Player player in activeTracked)
            {
                Contribution contribution = player.GetModPlayer<LeaderboardsPlayer>().contribution;
                packet.Write(player.whoAmI);
                foreach (string stat in Contribution.StatNames)
                    packet.Write((long)contribution.GetStat(stat));
            }
            packet.Send();
        }
    }
}
