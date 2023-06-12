using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace Leaderboards
{
    public static class LeaderboardsFunctions
    {
        public const int debug = 0;

        public static void PushContribution(Player player)
        {
            LeaderboardsPlayer leaderboardsPlayer = player.GetModPlayer<LeaderboardsPlayer>();

            foreach (KeyValuePair<string, Contribution> bossContribution in leaderboardsPlayer.contributions)
            {
                if (bossContribution.Value.damage > 0 || debug > 0)
                    Main.NewText(
                        player.name + " dealt " + bossContribution.Value.damage + " damage to " + bossContribution.Key + ".",
                        Color.Magenta
                    );

                if (bossContribution.Value.lifeLost > 0 || debug > 0)
                    Main.NewText(
                        player.name + " lost " + bossContribution.Value.lifeLost + " life to " + bossContribution.Key + ".",
                        Color.Red
                    );

                leaderboardsPlayer.contributions.Remove(bossContribution.Key);
            }
        }
    }
}
