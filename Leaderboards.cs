using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Leaderboards
{
    internal class Leaderboards : Mod
    {
        private int _recievedContributions = 0;

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                this.Logger.Debug($"Server recieved contribution from {Main.player[whoAmI].name} and is about to read packet.");
                long[] values = new long[5];
                values[0] = reader.ReadInt64();
                values[1] = reader.ReadInt64();
                values[2] = reader.ReadInt64();
                values[3] = reader.ReadInt64();
                values[4] = reader.ReadInt64();
                UpdateContribution(whoAmI, values);

                List<Player> activePlayers = Utilities.GetActivePlayers();
                if (++_recievedContributions >= activePlayers.Count)
                {
                    this.Logger.Debug($"Preparing large packet with {_recievedContributions} contributions.");
                    // Send large packet to all clients containing state of leaderboard
                    ModPacket packet = GetPacket();
                    packet.Write(activePlayers.Count);
                    foreach (Player player in activePlayers)
                    {
                        Contribution contribution = player.GetModPlayer<LeaderboardsPlayer>().contribution;
                        packet.Write(player.whoAmI);
                        packet.Write((long)contribution.GetStat("Damage"));
                        packet.Write((long)contribution.GetStat("Kills"));
                        packet.Write((long)contribution.GetStat("Life Lost"));
                        packet.Write((long)contribution.GetStat("Hits Taken"));
                        packet.Write((long)contribution.GetStat("Deaths"));
                    }
                    packet.Send();
                    _recievedContributions = 0;
                }
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                this.Logger.Info($"Client recieved packet from server and is about to read packet.");
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    int recievedWhoAmI = reader.ReadInt32();
                    long[] values = new long[5];
                    values[0] = reader.ReadInt64();
                    values[1] = reader.ReadInt64();
                    values[2] = reader.ReadInt64();
                    values[3] = reader.ReadInt64();
                    values[4] = reader.ReadInt64();
                    this.Logger.Debug($"Client read {Main.player[recievedWhoAmI].name} with values {values[0]}, {values[1]}, {values[2]}, {values[3]}, {values[4]}.");
                    UpdateContribution(recievedWhoAmI, values);
                }
            }
        }

        private void UpdateContribution(int whoAmI, long[] values)
        {
            this.Logger.Debug($"Updating contribution of {Main.player[whoAmI].name} with values {values[0]}, {values[1]}, {values[2]}, {values[3]}, {values[4]}.");
            LeaderboardsPlayer leaderboardsPlayer = Main.player[whoAmI].GetModPlayer<LeaderboardsPlayer>();
            leaderboardsPlayer.contribution = new Contribution(whoAmI);
            leaderboardsPlayer.contribution.SetStat("Damage", values[0]);
            leaderboardsPlayer.contribution.SetStat("Kills", values[1]);
            leaderboardsPlayer.contribution.SetStat("Life Lost", values[2]);
            leaderboardsPlayer.contribution.SetStat("Hits Taken", values[3]);
            leaderboardsPlayer.contribution.SetStat("Deaths", values[4]);
        }
    }
}
