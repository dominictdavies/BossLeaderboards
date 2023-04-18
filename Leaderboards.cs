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
                int contributionCount = reader.ReadInt32(); // Read the number of contributions
                packet.Write(contributionCount); // Write the number of contributions

                for (int i = 0; i < contributionCount; i++) {
                    packet.Write(reader.ReadString());
                    packet.Write(reader.ReadInt32());
                    packet.Write(reader.ReadInt32());
                }

                packet.Send(ignoreClient: whoAmI);
            } else {
                // Write out the received contribution
                Player contributor = Main.player[reader.ReadByte()];
                int contributionCount = reader.ReadInt32(); // Read the number of contributions

                for (int i = 0; i < contributionCount; i++) {
                    contributor.GetModPlayer<LeaderboardsPlayer>().bossContributions.Add(
                        reader.ReadString(),
                        new Contribution(reader.ReadInt32(), reader.ReadInt32())
                    );
                }

                LeaderboardsFunctions.PushContribution(contributor);
            }
        }
    }
}
