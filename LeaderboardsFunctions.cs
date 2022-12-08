using Microsoft.Xna.Framework;
using Terraria;

namespace Leaderboards
{
    public static class LeaderboardsFunctions
    {
        public static void NewContribution(Player player)
        {
            LeaderboardsPlayer leaderboardsPlayer = player.GetModPlayer<LeaderboardsPlayer>();

            Main.NewText(
                player.name + " contributed " + leaderboardsPlayer.contribution + " damage.",
                Color.Magenta
            );

            leaderboardsPlayer.contribution = 0;
        }
    }
}
