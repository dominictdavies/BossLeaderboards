using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class LifeTracker : GlobalNPC
    {
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

            Leaderboards.NewMessage(npc.FullName + " was hit!", Color.Orange);
            Leaderboards.NewMessage("  Damage: " + damage);
            Leaderboards.NewMessage("  Knockback: " + knockback);
            Leaderboards.NewMessage("  Crit: " + crit);
            Leaderboards.NewMessage("  Life lost: " + lifeLost);
            Leaderboards.NewMessage("  Total life lost: " + totalLifeLost);

            if (player != default) {
                Leaderboards.NewMessage("  Player: " + player.name);
                Leaderboards.NewMessage("  Item: " + item.Name);
            }

            if (projectile != default) {
                Leaderboards.NewMessage("  Projectile name: " + projectile.Name);
                Leaderboards.NewMessage("  Projectile owner: " + Main.player[projectile.owner].name);
            }

            if (npc.life <= 0) {
                Leaderboards.NewMessage(npc.FullName + " has died!", Color.Red);
            }
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
            => OnHitByAnything(npc, damage, knockback, crit, player:player, item:item);

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
            => OnHitByAnything(npc, damage, knockback, crit, projectile:projectile);
    }
}
