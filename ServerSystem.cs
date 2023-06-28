using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Leaderboards
{
    internal class ServerSystem : ModSystem
    {
        private const int PacketTimerMax = 30;
        private int _packetTimer = PacketTimerMax;

        public override void PostUpdateEverything()
        {
            if (Main.netMode != NetmodeID.Server)
                return;

            if (--_packetTimer == 0)
            {
                // Send large packet to all clients containing state of leaderboard
                List<Player> activePlayers = Utilities.GetActivePlayers();
                ModPacket packet = Mod.GetPacket();
                packet.Write(activePlayers.Count);
                foreach (Player player in activePlayers)
                {
                    Contribution contribution = player.GetModPlayer<LeaderboardsPlayer>().contribution;
                    packet.Write(player.whoAmI);
                    packet.Write((long)contribution.GetStat("Damage"));
                    packet.Write((long)contribution.GetStat("Kills"));
                    packet.Write((long)contribution.GetStat("Life Lost"));
                    packet.Write((long)contribution.GetStat("Hits Taken"));
                    packet.Write((long)contribution.GetStat("Deaths"));
                }
                packet.Send();
                Mod.Logger.Info($"Sent large packet.");
                _packetTimer = PacketTimerMax;
            }
        }
    }
}
