using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class LeaderboardsPlayer : ModPlayer
    {
        private bool debug = true;

        public void OnHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = default, Projectile proj = default)
        {
            if (debug) {
                if (item != default) {
                    Leaderboards.NewMessage(
                        target.FullName + " was hit by " + Player.name + " with " + item.Name, Color.Orange
                    );
                }

                if (proj != default) {
                    Leaderboards.NewMessage(
                        target.FullName + " was hit by " + Main.player[proj.owner].name + " with " + proj.Name, Color.Orange
                    );
                }

                Leaderboards.NewMessage("  Damage: " + damage);
                Leaderboards.NewMessage("  Knockback: " + knockback);
                Leaderboards.NewMessage("  Crit: " + crit);

                if (target.life <= 0) {
                    Leaderboards.NewMessage(target.FullName + " was killed!", Color.Purple);
                }
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
            => OnHitNPCWithAnything(target, damage, knockback, crit, item: item);

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
            => OnHitNPCWithAnything(target, damage, knockback, crit, proj: proj);
    }
}
