using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Leaderboards
{
    internal class Leaderboards : Mod
    {
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                // Send packet to other clients containing sender and contribution
                ModPacket packet = GetPacket();
                packet.Write((byte)whoAmI);
                packet.Write(reader.ReadInt64()); // damage
                packet.Write(reader.ReadInt64()); // kills
                packet.Write(reader.ReadInt64()); // lifeLost
                packet.Write(reader.ReadInt64()); // hitsTaken
                packet.Write(reader.ReadInt64()); // deaths
                packet.Send(ignoreClient: whoAmI);
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Update player's contribution
                Player contributor = Main.player[reader.ReadByte()]; // whoAmI
                Contribution contribution = contributor.GetModPlayer<LeaderboardsPlayer>().contribution;
                contribution.SetStat("Damage", reader.ReadInt64());
                contribution.SetStat("Kills", reader.ReadInt64());
                contribution.SetStat("Life Lost", reader.ReadInt64());
                contribution.SetStat("Hits Taken", reader.ReadInt64());
                contribution.SetStat("Deaths", reader.ReadInt64());
            }
        }
    }
}
