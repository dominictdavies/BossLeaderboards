using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace Leaderboards
{
    public static class LeaderboardsFunctions
    {
        public static void PushContribution(Player player)
        {
            LeaderboardsPlayer leaderboardsPlayer = player.GetModPlayer<LeaderboardsPlayer>();

            foreach (KeyValuePair<string, Contribution> pair in leaderboardsPlayer.bossContributions) {
                Main.NewText(
                    player.name + " contributed " + pair.Value.totalDamage + " damage to " + pair.Key + ".",
                    Color.Magenta
                );

                leaderboardsPlayer.bossContributions.Remove(pair.Key);
            }
        }
    }
}
