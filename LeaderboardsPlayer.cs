using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class LeaderboardsPlayer : ModPlayer
    {
        public int contribution = 0;
        public bool share = false;

        public override void OnEnterWorld(Player player)
        {
            contribution = 0;
            share = false;
        }

        public override void ResetEffects()
        {
            if (share) {
                Main.NewText(
                    Player.name + " dealt " + contribution + " during the boss fight.",
                    Color.Aqua
                );
                contribution = 0;
                share = false;
            }
        }

        public void OnHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = default, Projectile proj = default)
        {
            contribution += damage;

            if (Leaderboards.debug) {
                //Main.NewText("  Damage: " + damage);
                //Main.NewText("  Knockback: " + knockback);
                //Main.NewText("  Crit: " + crit);
                Main.NewText("  Contribution = " + contribution);
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
            => OnHitNPCWithAnything(target, damage, knockback, crit, item: item);

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
            => OnHitNPCWithAnything(target, crit ? damage * 2 : damage, knockback, crit, proj: proj);
    }
}
