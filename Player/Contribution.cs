using Leaderboards.UI;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace Leaderboards
{
    internal class Contribution
    {
        private int whoAmI;
        private Dictionary<string, object> contribution = new();

        public Contribution(int whoAmI)
        {
            this.whoAmI = whoAmI;
            foreach (string statName in UILeaderboard.Stats)
                contribution.Add(statName, 0L);
        }

        public void AddThisPlayer()
        {
            ModContent.GetInstance<UILeaderboardSystem>().leaderboard.AddPlayer(whoAmI);
        }

        public object GetStat(string statName) => contribution[statName];

        public void SetStat(string statName, object value)
        {
            contribution[statName] = value;

            UILeaderboard leaderboard = ModContent.GetInstance<UILeaderboardSystem>().leaderboard;
            leaderboard.UpdateCell(whoAmI, statName, contribution[statName]);
        }

        public void PlusStat(string statName, long value)
        {
            contribution[statName] = (long)contribution[statName] + value;

            UILeaderboard leaderboard = ModContent.GetInstance<UILeaderboardSystem>().leaderboard;
            leaderboard.UpdateCell(whoAmI, statName, contribution[statName]);
        }
    }
}
