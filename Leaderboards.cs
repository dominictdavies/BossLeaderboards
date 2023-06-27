using System.IO;
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
                UpdateContribution(reader);

                if (++_recievedContributions >= Utilities.GetActivePlayers().Count)
                {
                    // Send packet to other clients containing sender and contribution
                    ModPacket packet = GetPacket();
                    packet.Write((byte)whoAmI);
                    packet.Write(reader.ReadInt64()); // damage
                    packet.Write(reader.ReadInt64()); // kills
                    packet.Write(reader.ReadInt64()); // lifeLost
                    packet.Write(reader.ReadInt64()); // hitsTaken
                    packet.Write(reader.ReadInt64()); // deaths
                    packet.Send();
                    _recievedContributions = 0;
                }
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {

            }
        }

        private void UpdateContribution(BinaryReader reader)
        {
            Player player = Main.player[reader.ReadByte()]; // whoAmI
            Contribution contribution = player.GetModPlayer<LeaderboardsPlayer>().contribution;
            contribution.SetStat("Damage", reader.ReadInt64());
            contribution.SetStat("Kills", reader.ReadInt64());
            contribution.SetStat("Life Lost", reader.ReadInt64());
            contribution.SetStat("Hits Taken", reader.ReadInt64());
            contribution.SetStat("Deaths", reader.ReadInt64());
        }
    }
}
