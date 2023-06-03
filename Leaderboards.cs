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
                // Send packet to other clients containing sender and contribution
                ModPacket packet = GetPacket();
                packet.Write((byte)whoAmI);
                int contributionCount = reader.ReadInt32();
                packet.Write(contributionCount);

                for (int i = 0; i < contributionCount; i++) {
                    packet.Write(reader.ReadString()); // NPC name
                    packet.Write(reader.ReadInt32()); // totalDamageTo
                    packet.Write(reader.ReadInt32()); // totalLifeLostFrom
                }

                packet.Send(ignoreClient: whoAmI);
            } else if (Main.netMode == NetmodeID.MultiplayerClient) {
                // Push received contribution
                Player contributor = Main.player[reader.ReadByte()]; // whoAmI
                int contributionCount = reader.ReadInt32();

                for (int i = 0; i < contributionCount; i++) {
                    contributor.GetModPlayer<LeaderboardsPlayer>().contributions.Add(
                        reader.ReadString(), // NPC name
                        new Contribution(
                            reader.ReadInt32(), // totalDamageTo
                            reader.ReadInt32() // totalLifeLostFrom
                        )
                    );
                }

                LeaderboardsFunctions.PushContribution(contributor);
            }
        }
    }
}
