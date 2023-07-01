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
        public Contribution contribution = new Contribution();

        public override void OnEnterWorld(Player player)
        {
            UILeaderboard leaderboard = ModContent.GetInstance<LeaderboardSystem>().leaderboard;
            leaderboard.RemoveData(true);
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (Main.CurrentFrameFlags.AnyActiveBossNPC)
                ModContent.GetInstance<LeaderboardSystem>().ShowMyUI(playSound: false);

            return true;
        }

        public void PreHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = null, Projectile proj = null)
            => _targetOldLife = target.life;

        public void PreHitByAnything(int damage, bool crit, NPC npc = null, Projectile proj = null)
            => _playerOldLife = Player.statLife;

        public void PostHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = null, Projectile proj = null)
        {
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            int damageDealt = target.life > 0 ? _targetOldLife - target.life : _targetOldLife;
            if (damageDealt > 0)
                contribution.IncreaseStat(Player.whoAmI, "Damage", damageDealt);

            if (target.life <= 0 && _targetOldLife > 0)
                contribution.IncreaseStat(Player.whoAmI, "Kills");
        }

        public void PostHitByAnything(int damage, bool crit, NPC npc = null, Projectile proj = null)
        {
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            int lifeLost = Player.statLife > 0 ? _playerOldLife - Player.statLife : _playerOldLife;
            if (lifeLost > 0)
            {
                contribution.IncreaseStat(Player.whoAmI, "Life Lost", lifeLost);
                contribution.IncreaseStat(Player.whoAmI, "Hits Taken");
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            contribution.IncreaseStat(Player.whoAmI, "Deaths");
        }
    }
}
