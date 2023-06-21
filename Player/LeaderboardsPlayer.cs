using Leaderboards.UI;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Leaderboards
{
    internal partial class LeaderboardsPlayer : ModPlayer
    {
        private int _targetOldLife;
        private int _playerOldLife;
        public Contribution contribution;

        public override void OnEnterWorld(Player player)
        {
            UILeaderboardSystem leaderboardSystem = ModContent.GetInstance<UILeaderboardSystem>();
            UILeaderboard leaderboard = leaderboardSystem.leaderboard;

            leaderboard.ClearData();
            leaderboardSystem.ShowMyUI();
        }

        public void PreHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = null, Projectile proj = null)
            => _targetOldLife = target.life;

        public void PostHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = null, Projectile proj = null)
        {
            if (contribution is null)
                return;

            int damageDealt = target.life > 0 ? _targetOldLife - target.life : _targetOldLife;
            if (damageDealt > 0)
                contribution.PlusStat("Damage", damageDealt);

            if (target.life <= 0 && _targetOldLife > 0)
                contribution.IncrementStat("Kills");
        }

        public void PreHitByAnything(int damage, bool crit, NPC npc = null, Projectile proj = null)
            => _playerOldLife = Player.statLife;

        public void PostHitByAnything(int damage, bool crit, NPC npc = null, Projectile proj = null)
        {
            if (contribution is null)
                return;

            int lifeLost = Player.statLife > 0 ? _playerOldLife - Player.statLife : _playerOldLife;
            if (lifeLost > 0)
            {
                contribution.PlusStat("Life Lost", lifeLost);
                contribution.IncrementStat("Hits Taken");
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (contribution is null)
                return;

            contribution.IncrementStat("Deaths");
        }
    }
}
