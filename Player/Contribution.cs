using Leaderboards.UI;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    internal class Contribution
    {
        private Player player;
        private Dictionary<string, object> contribution = new();

        public Contribution(Player player)
        {
            this.player = player;
            this.contribution.Add("Player", player.name);
            foreach (string stat in UILeaderboard.Stats)
                this.contribution.Add(stat, 0L);
        }

        public object GetStat(string statName) => contribution[statName];

        public void SetStat(string statName, object value)
        {
            contribution[statName] = value;

            UILeaderboard leaderboard = ModContent.GetInstance<UILeaderboardSystem>().leaderboard;
            leaderboard.UpdateCell(player.whoAmI, statName, contribution[statName]);
        }

        public void PlusStat(string statName, long value)
        {
            contribution[statName] = (long)contribution[statName] + value;

            UILeaderboard leaderboard = ModContent.GetInstance<UILeaderboardSystem>().leaderboard;
            leaderboard.UpdateCell(player.whoAmI, statName, contribution[statName]);
        }
    }
}
