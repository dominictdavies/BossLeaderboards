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
                if (reader.ReadBoolean())
                {
                    ModPacket packet = this.GetPacket();
                    packet.Write(1);
                    packet.Write(whoAmI);
                    packet.Write(reader.ReadInt64());
                    packet.Write(reader.ReadInt64());
                    packet.Write(reader.ReadInt64());
                    packet.Write(reader.ReadInt64());
                    packet.Write(reader.ReadInt64());
                    packet.Send(ignoreClient: whoAmI);
                }
                else
                {
                    this.Logger.Debug($"Recieved packet from {Main.player[whoAmI].name}.");
                    long[] values = new long[5];
                    values[0] = reader.ReadInt64();
                    values[1] = reader.ReadInt64();
                    values[2] = reader.ReadInt64();
                    values[3] = reader.ReadInt64();
                    values[4] = reader.ReadInt64();
                    UpdateContribution(whoAmI, values);
                }
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                this.Logger.Debug("Recieved packet from Server.");
                int playerCount = reader.ReadInt32();
                for (int i = 0; i < playerCount; i++)
                {
                    int recievedWhoAmI = reader.ReadInt32();
                    long[] values = new long[5];
                    values[0] = reader.ReadInt64();
                    values[1] = reader.ReadInt64();
                    values[2] = reader.ReadInt64();
                    values[3] = reader.ReadInt64();
                    values[4] = reader.ReadInt64();
                    if (recievedWhoAmI == Main.myPlayer)
                    {
                        this.Logger.Debug("Recieved own contribution so will ignore it.");
                        continue;
                    }

                    this.Logger.Debug($"Read {Main.player[recievedWhoAmI].name} with values {values[0]}, {values[1]}, {values[2]}, {values[3]}, {values[4]}.");
                    UpdateContribution(recievedWhoAmI, values);
                }
            }
        }

        private void UpdateContribution(int whoAmI, long[] values)
        {
            LeaderboardsPlayer leaderboardsPlayer = Main.player[whoAmI].GetModPlayer<LeaderboardsPlayer>();
            leaderboardsPlayer.contribution = new Contribution(whoAmI);
            leaderboardsPlayer.contribution.SetStat("Damage", values[0]);
            leaderboardsPlayer.contribution.SetStat("Kills", values[1]);
            leaderboardsPlayer.contribution.SetStat("Life Lost", values[2]);
            leaderboardsPlayer.contribution.SetStat("Hits Taken", values[3]);
            leaderboardsPlayer.contribution.SetStat("Deaths", values[4]);
            this.Logger.Debug($"Updated contribution of {Main.player[whoAmI].name} with values {values[0]}, {values[1]}, {values[2]}, {values[3]}, {values[4]}.");
        }
    }
}
