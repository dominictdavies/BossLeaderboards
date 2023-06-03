using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace Leaderboards
{
    public static class LeaderboardsFunctions
    {
        public const int debug = 1;

        public static void PushContribution(Player player)
        {
            LeaderboardsPlayer leaderboardsPlayer = player.GetModPlayer<LeaderboardsPlayer>();

            foreach (KeyValuePair<string, Contribution> bossContribution in leaderboardsPlayer.contributions) {
                Main.NewText(
                    player.name + " dealt " + bossContribution.Value.totalDamageTo + " damage to " + bossContribution.Key + ".",
                    Color.Magenta
                );

                Main.NewText(
                    player.name + " lost " + bossContribution.Value.totalLifeLostFrom + " life to " + bossContribution.Key + ".",
                    Color.Red
                );

                leaderboardsPlayer.contributions.Remove(bossContribution.Key);
            }
        }
    }
}
