using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    internal class ServerSystem : ModSystem
    {
        private const int PacketTimerMax = 30;
        private int _packetTimer = PacketTimerMax;
        private const int StallTimerMax = 5 * 60;
        private int _stallTimer = 0;
        private bool _oldAnyActiveBossNPC = false;
        private List<Player> _trackedPlayers;

        public override void PreUpdateWorld()
        {
            if (Main.CurrentFrameFlags.AnyActiveBossNPC || --_stallTimer > 0)
            {
                if (Main.CurrentFrameFlags.AnyActiveBossNPC && !_oldAnyActiveBossNPC) // Boss just spawned 
                {
                    _trackedPlayers = Utilities.GetActivePlayers();
                    foreach (Player player in _trackedPlayers)
                        player.GetModPlayer<LeaderboardsPlayer>().contribution = new Contribution();
                }

                if (--_packetTimer == 0)
                {
                    // Send large packet to all clients containing state of leaderboard
                    List<Player> activeTracked = _trackedPlayers.Where(player => player.active).ToList();
                    ModPacket packet = Mod.GetPacket();
                    packet.Write(activeTracked.Count);
                    foreach (Player player in activeTracked)
                    {
                        Contribution contribution = player.GetModPlayer<LeaderboardsPlayer>().contribution;
                        packet.Write(player.whoAmI);
                        foreach (string stat in Contribution.StatNames)
                            packet.Write((long)contribution.GetStat(stat));
                    }
                    packet.Send();
                    _packetTimer = PacketTimerMax;
                }
            }
            else if (!Main.CurrentFrameFlags.AnyActiveBossNPC && _oldAnyActiveBossNPC) // Boss just died
            {
                _stallTimer = StallTimerMax;
            }

            _oldAnyActiveBossNPC = Main.CurrentFrameFlags.AnyActiveBossNPC;
        }
    }
}
