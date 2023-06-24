using System.Collections.Generic;
using Terraria;

namespace Leaderboards
{
    public static class Utilities
    {
        public static List<Player> GetActivePlayers()
        {
            List<Player> activePlayers = new List<Player>();
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active)
                    activePlayers.Add(player);
            }
            return activePlayers;
        }
    }
}
