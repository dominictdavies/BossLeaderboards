using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Leaderboards
{
    internal class Leaderboards : Mod
    {
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                UpdateContribution(reader, whoAmI);
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                int playerCount = reader.ReadInt32();
                for (int i = 0; i < playerCount; i++)
                {
                    int recievedWhoAmI = reader.ReadInt32();
                    if (recievedWhoAmI == Main.myPlayer)
                    {
                        reader.BaseStream.Position += Contribution.StatNames.Length * sizeof(long);
                        continue;
                    }
                    UpdateContribution(reader, recievedWhoAmI);
                }
            }
        }

        private void UpdateContribution(BinaryReader reader, int whoAmI)
        {
            LeaderboardsPlayer leaderboardsPlayer = Main.player[whoAmI].GetModPlayer<LeaderboardsPlayer>();
            foreach (string statName in Contribution.StatNames)
                leaderboardsPlayer.contribution.SetStat(whoAmI, statName, reader.ReadInt64());
        }
    }
}
