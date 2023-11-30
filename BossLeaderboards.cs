using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace BossLeaderboards
{
    internal class BossLeaderboards : Mod
    {
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                UpdateContribution(reader, whoAmI);
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                int trackedCount = reader.ReadInt32();
                for (int i = 0; i < trackedCount; i++)
                {
                    int recievedWhoAmI = reader.ReadInt32();
                    if (recievedWhoAmI == Main.myPlayer)
                        reader.BaseStream.Position += Contribution.StatNames.Length * sizeof(long);
                    else
                        UpdateContribution(reader, recievedWhoAmI);
                }
            }
        }

        private void UpdateContribution(BinaryReader reader, int whoAmI)
        {
            Contribution contribution = Main.player[whoAmI].GetModPlayer<LeaderboardsPlayer>().contribution;
            foreach (string statName in Contribution.StatNames)
                contribution.SetStat(whoAmI, statName, reader.ReadInt64());
        }
    }
}
