using Leaderboards.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    public static class LeaderboardsFunctions
    {
        public const int debug = 0;

        public static void PushContribution(Player player)
        {
            LeaderboardsPlayer leaderboardsPlayer = player.GetModPlayer<LeaderboardsPlayer>();
            Contribution contribution = leaderboardsPlayer.contribution;

            if (debug > 0)
                Main.NewTextMultiline(
                    player.name + " dealt " + contribution.damage + " damage.\n" +
                    player.name + " killed " + contribution.kills + " enemies.\n" +
                    player.name + " lost " + contribution.lifeLost + " life.\n" +
                    player.name + " took " + contribution.hitsTaken + " hits.\n" +
                    player.name + " died " + contribution.deaths + " times.",
                    c: Color.Magenta
                );

            ModContent.GetInstance<LeaderboardSystem>().leaderboard.AddContribution(player.whoAmI, contribution);
            contribution.Reset();
        }
    }
}
