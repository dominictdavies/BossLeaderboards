using Terraria;
using Terraria.ModLoader;

namespace BossLeaderboards
{
    internal partial class LeaderboardsPlayer : ModPlayer
    {
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
            => OnHitNPCWithAnything(target, hit, damageDone, item: item);

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
            => OnHitNPCWithAnything(target, hit, damageDone, proj: proj);

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
            => OnHitByAnything(hurtInfo, npc: npc);

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
            => OnHitByAnything(hurtInfo, proj: proj);
    }
}
