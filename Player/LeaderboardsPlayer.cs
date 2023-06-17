using Leaderboards.UI;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Leaderboards
{
    internal partial class LeaderboardsPlayer : ModPlayer
    {
        private int targetOldLife;
        private int playerOldLife;
        public Contribution contribution;

        public override void OnEnterWorld(Player player)
        {
            UILeaderboardSystem leaderboardSystem = ModContent.GetInstance<UILeaderboardSystem>();
            UILeaderboard leaderboard = leaderboardSystem.leaderboard;

            leaderboard.ClearData();
            leaderboardSystem.ShowMyUI();
        }

        public void PreHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = null, Projectile proj = null)
            => targetOldLife = target.life;

        public void PostHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = null, Projectile proj = null)
        {
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            int damageDealt = target.life > 0 ? targetOldLife - target.life : targetOldLife;
            if (damageDealt > 0)
                contribution.PlusStat("Damage", damageDealt);

            if (target.life <= 0 && targetOldLife > 0)
                contribution.PlusStat("Kills", 1);
        }

        public void PreHitByAnything(int damage, bool crit, NPC npc = null, Projectile proj = null)
            => playerOldLife = Player.statLife;

        public void PostHitByAnything(int damage, bool crit, NPC npc = null, Projectile proj = null)
        {
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            int lifeLost = Player.statLife > 0 ? playerOldLife - Player.statLife : playerOldLife;
            if (lifeLost > 0)
            {
                contribution.PlusStat("Life Lost", lifeLost);
                contribution.PlusStat("Hits Taken", 1);
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            contribution.PlusStat("Deaths", 1);
        }
    }
}
