using Leaderboards.UI;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace Leaderboards
{
    internal class Contribution
    {
        private int _whoAmI;
        private Dictionary<string, object> _contribution;

        public Contribution(int whoAmI)
        {
            this._whoAmI = whoAmI;
            this._contribution = new Dictionary<string, object>();
            foreach (string statName in UILeaderboard.Stats)
                _contribution.Add(statName, 0L);

            ModContent.GetInstance<UILeaderboardSystem>().leaderboard.AddPlayer(whoAmI, _contribution);
        }

        public object GetStat(string statName) => _contribution[statName];

        public void SetStat(string statName, object value)
        {
            _contribution[statName] = value;
            UpdateCell(statName);
        }

        public void PlusStat(string statName, long value)
        {
            _contribution[statName] = (long)_contribution[statName] + value;
            UpdateCell(statName);
        }

        private void UpdateCell(string statName)
            => ModContent.GetInstance<UILeaderboardSystem>().leaderboard.UpdateCell(_whoAmI, statName, _contribution[statName]);
    }
}
