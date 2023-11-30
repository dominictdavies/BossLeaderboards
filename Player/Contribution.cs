using BossLeaderboards.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace BossLeaderboards
{
    internal class Contribution
    {
        public static string[] StatNames = { "Damage", "Kills", "Life Lost", "Hits Taken", "Deaths" };
        private Dictionary<string, object> _contribution;

        public Contribution()
        {
            this._contribution = new Dictionary<string, object>();
            Reset();
        }

        public void Reset()
        {
            this._contribution.Clear();
            foreach (string statName in StatNames)
                _contribution.Add(statName, 0L);
        }

        public object GetStat(string statName) => _contribution[statName];

        public void IncreaseStat(int whoAmI, string statName, long amount = 1)
            => SetStat(whoAmI, statName, (long)_contribution[statName] + amount);

        public void SetStat(int whoAmI, string statName, object value)
        {
            _contribution[statName] = value;
            if (Main.netMode != NetmodeID.Server)
                ModContent.GetInstance<LeaderboardSystem>().leaderboard.UpdateCell(whoAmI, statName, value);
        }
    }
}
