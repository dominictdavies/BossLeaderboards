using Microsoft.Xna.Framework;
using Terraria;

namespace Leaderboards
{
    public static class LeaderboardsFunctions
    {
        public const int debug = 1;

        public static void PushContribution(Player player)
        {
            LeaderboardsPlayer leaderboardsPlayer = player.GetModPlayer<LeaderboardsPlayer>();
            Contribution contribution = leaderboardsPlayer.contribution;

            if (contribution.damage > 0 || debug > 0)
                Main.NewText(
                    player.name + " dealt " + contribution.damage + " damage.\n" +
                    player.name + " killed " + contribution.kills + " enemies.\n" +
                    player.name + " lost " + contribution.lifeLost + " life.\n" +
                    player.name + " took " + contribution.hitsTaken + " hits.\n" +
                    player.name + " died " + contribution.deaths + " times.\n",
                    Color.Magenta
                );

            contribution.Reset();
        }
    }
}
