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
                ModPacket packet = GetPacket();
                packet.Write((byte)whoAmI);
                packet.Write(reader.ReadInt32());
                packet.Send(ignoreClient: whoAmI);
            } else {
                Player contributor = Main.player[reader.ReadByte()];
                int contribution = reader.ReadInt32();
                LeaderboardsFunctions.SendContribution(contributor, contribution);
            }
        }
    }
}
