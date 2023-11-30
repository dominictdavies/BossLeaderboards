using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace BossLeaderboards.Source
{
    public static class Utilities
    {
        public static List<Player> GetActivePlayers(Player[] players)
        {
            var activePlayers = players.Where(player => player.active).ToList();
            return activePlayers;
        }
    }
}
