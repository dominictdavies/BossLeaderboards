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

            foreach (KeyValuePair<string, Contribution> bossContribution in leaderboardsPlayer.bossContributions) {
                Main.NewText(
                    player.name + " contributed " + bossContribution.Value.totalDamage + " damage to " + bossContribution.Key + ".",
                    Color.Magenta
                );

                Main.NewText(
                    player.name + " lost " + bossContribution.Value.totalLifeLost + " life to " + bossContribution.Key + ".",
                    Color.Red
                );

                leaderboardsPlayer.bossContributions.Remove(bossContribution.Key);
            }
        }
    }
}
