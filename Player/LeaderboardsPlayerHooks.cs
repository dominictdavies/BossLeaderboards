using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    internal partial class LeaderboardsPlayer : ModPlayer
    {
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Item, consider using ModifyHitNPC instead */
            => PreHitNPCWithAnything(target, damage, knockback, crit, item: item);

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Projectile, consider using ModifyHitNPC instead */
            => PreHitNPCWithAnything(target, damage, knockback, crit, proj: proj);

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
            => PostHitNPCWithAnything(target, damage, knockback, crit, item: item);

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
            => PostHitNPCWithAnything(target, damage, knockback, crit, proj: proj);

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
            => PreHitByAnything(damage, crit, npc: npc);

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
            => PreHitByAnything(damage, crit, proj: proj);

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
            => PostHitByAnything(damage, crit, npc: npc);

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
            => PostHitByAnything(damage, crit, proj: proj);
    }
}
