using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class Leaderboards : Mod
    {
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            if (Main.netMode == NetmodeID.Server) {
                // Send new packet containing sender and contribution to others
                ModPacket packet = GetPacket();
                packet.Write((byte)whoAmI);
                packet.Write(reader.ReadInt32());
                packet.Send(ignoreClient: whoAmI);
            } else {
                // Write out the received contribution
                Player contributor = Main.player[reader.ReadByte()];
                contributor.GetModPlayer<LeaderboardsPlayer>().contribution = reader.ReadInt32();
                LeaderboardsFunctions.NewContribution(contributor);
            }
        }
    }
}
