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
                packet.Write(reader.ReadInt32()); // kills
                packet.Write(reader.ReadInt64()); // lifeLost
                packet.Write(reader.ReadInt32()); // hitsTaken
                packet.Write(reader.ReadInt32()); // deaths
                packet.Send(ignoreClient: whoAmI);
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Update player's contribution
                Player contributor = Main.player[reader.ReadByte()]; // whoAmI
                contributor.GetModPlayer<LeaderboardsPlayer>().contribution = new(
                    reader.ReadInt64(), // damage
                    reader.ReadInt32(), // kills
                    reader.ReadInt64(), // lifeLost
                    reader.ReadInt32(), // hitsTaken
                    reader.ReadInt32()  // deaths
                );
            }
        }
    }
}
