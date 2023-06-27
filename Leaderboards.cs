using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Leaderboards
{
    internal class Leaderboards : Mod
    {
        private byte _recievedContributions = 0;

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                long[] values = new long[] {
                    reader.ReadInt64(),
                    reader.ReadInt64(),
                    reader.ReadInt64(),
                    reader.ReadInt64(),
                    reader.ReadInt64()
                };
                UpdateContribution(Main.player[whoAmI], values);

                List<Player> activePlayers = Utilities.GetActivePlayers();
                if (++_recievedContributions >= activePlayers.Count)
                {
                    // Send large packet to all clients containing state of leaderboard
                    ModPacket packet = GetPacket();
                    packet.Write((byte)activePlayers.Count);
                    foreach (Player player in activePlayers)
                    {
                        Contribution contribution = player.GetModPlayer<LeaderboardsPlayer>().contribution;
                        packet.Write((byte)player.whoAmI);
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
                for (byte i = 0; i < reader.ReadByte(); i++)
                {
                    byte playerIndex = reader.ReadByte();
                    long[] values = new long[] {
                        reader.ReadInt64(),
                        reader.ReadInt64(),
                        reader.ReadInt64(),
                        reader.ReadInt64(),
                        reader.ReadInt64()
                    };
                    UpdateContribution(Main.player[playerIndex], values);
                }
            }
        }

        private void UpdateContribution(Player player, long[] values)
        {
            Contribution contribution = player.GetModPlayer<LeaderboardsPlayer>().contribution;
            contribution.SetStat("Damage", values[0]);
            contribution.SetStat("Kills", values[1]);
            contribution.SetStat("Life Lost", values[2]);
            contribution.SetStat("Hits Taken", values[3]);
            contribution.SetStat("Deaths", values[4]);
        }
    }
}
