using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Leaderboards
{
    internal partial class LeaderboardsPlayer : ModPlayer
    {
        public Contribution contribution = new();
        public int targetOldLife;
        public int playerOldLife;

        public void PreHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = null, Projectile proj = null)
            => targetOldLife = target.life;

        public void PostHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = null, Projectile proj = null)
        {
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            int damageDealt = target.life > 0 ? targetOldLife - target.life : targetOldLife;
            if (damageDealt > 0)
                contribution.damage += damageDealt;

            if (target.life <= 0 && targetOldLife > 0)
                contribution.kills++;
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
                contribution.lifeLost += lifeLost;
                contribution.hitsTaken++;
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            contribution.deaths++;
        }
    }
}
