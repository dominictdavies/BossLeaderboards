using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    public partial class LeaderboardsPlayer : ModPlayer
    {
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
            => PreHitNPCWithAnything(target, damage, knockback, crit, item: item);

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
            => PreHitNPCWithAnything(target, damage, knockback, crit, proj: proj);

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
            => PostHitNPCWithAnything(target, damage, knockback, crit, item: item);

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
            => PostHitNPCWithAnything(target, damage, knockback, crit, proj: proj);

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
            => PreHitByAnything(damage, crit, npc: npc);

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
            => PreHitByAnything(damage, crit, proj: proj);

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
            => PostHitByAnything(damage, crit, npc: npc);

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
            => PostHitByAnything(damage, crit, proj: proj);
    }
}
