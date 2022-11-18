using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class LeaderboardsPlayer : ModPlayer
    {
        public int[] contributions = new int[Main.npc.Length];

        public void OnHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = default, Projectile proj = default)
        {
            contributions[target.whoAmI] += damage;

            if (Leaderboards.debug) {
                if (item != default) Main.NewText(
                        target.FullName + " was hit by " + Player.name + " with " + item.Name,
                        Color.Orange
                    );

                if (proj != default) Main.NewText(
                        target.FullName + " was hit by " + Main.player[proj.owner].name + " with " + proj.Name,
                        Color.Orange
                    );

                //Main.NewText("  Damage: " + damage);
                //Main.NewText("  Knockback: " + knockback);
                //Main.NewText("  Crit: " + crit);

                for (int i = 0; i < Main.npc.Length; i++) {
                    if (contributions[i] != 0) {
                        Main.NewText("contributions[" + i + "] = " + contributions[i]);
                    }
                }

                if (target.life <= 0) {
                    Main.NewText(target.FullName + " was killed!", Color.Purple);
                    contributions[target.whoAmI] = 0;

                    //foreach (Player player in Main.player) {
                    //    if (player.active) {
                    //        int[] contributions = player.GetModPlayer<LeaderboardsPlayer>().contributions;
                    //        if (contributions[npc.whoAmI] != 0) {
                    //            Main.NewText(
                    //                player.name + " dealt " + contributions[npc.whoAmI].ToString() + " damage to " + npc.FullName,
                    //                Color.Yellow
                    //            );
                    //            contributions[npc.whoAmI] = 0;
                    //        }
                    //    }
                    //}
                }
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
            => OnHitNPCWithAnything(target, damage, knockback, crit, item: item);

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
            => OnHitNPCWithAnything(target, crit ? damage * 2 : damage, knockback, crit, proj: proj);
    }
}
