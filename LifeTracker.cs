using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class LifeTracker : GlobalNPC
    {
        private bool debug = true;
        public int oldLife;
        public int totalLifeLost = 0;

        public override bool InstancePerEntity => true;

        public override bool PreAI(NPC npc)
        {
            oldLife = npc.life;
            return true;
        }

        public void OnHitByAnything(NPC npc, int damage, float knockback, bool crit, Player player = default, Item item = default, Projectile projectile = default)
        {
            int lifeLost = oldLife - npc.life;
            totalLifeLost += lifeLost;

            if (debug) {
                if (player != default) {
                    Leaderboards.NewMessage(
                        npc.FullName + " was hit by " + player.name + " with " + item.Name, Color.Orange
                    );
                }

                if (projectile != default) {
                    Leaderboards.NewMessage(
                        npc.FullName + " was hit by " + Main.player[projectile.owner].name + " with " + projectile.Name, Color.Orange
                    );
                }

                Leaderboards.NewMessage("  Damage: " + damage);
                Leaderboards.NewMessage("  Knockback: " + knockback);
                Leaderboards.NewMessage("  Crit: " + crit);
                Leaderboards.NewMessage("  Life lost: " + lifeLost);
                Leaderboards.NewMessage("  Total life lost: " + totalLifeLost);

                if (npc.life <= 0) {
                    Leaderboards.NewMessage(npc.FullName + " was killed!", Color.Purple);
                }
            }
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
            => OnHitByAnything(npc, damage, knockback, crit, player:player, item:item);

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
            => OnHitByAnything(npc, damage, knockback, crit, projectile:projectile);
    }
}
