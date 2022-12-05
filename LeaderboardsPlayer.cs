using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class LeaderboardsPlayer : ModPlayer
    {
        public int[] contributions = new int[Main.npc.Length];
        public List<int> shareContributions = new List<int>();

        public override void OnEnterWorld(Player player)
        {
            contributions = new int[Main.npc.Length];
            shareContributions = new List<int>();
        }

        public override void ResetEffects()
        {
            while (shareContributions.Count > 0) {
                int contribution = shareContributions[0];
                Main.NewText(Player.name + " dealt " + contributions[contribution] + " damage to " + Main.npc[contribution].FullName, Color.Aqua);
                contributions[contribution] = 0;
                shareContributions.RemoveAt(0);
            }
        }

        public void OnHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = default, Projectile proj = default)
        {
            contributions[target.whoAmI] += damage;

            if (Leaderboards.debug) {
                if (item != default) Main.NewText(
                        target.FullName + " was hit by " + Player.name + " with " + item.Name,
                        Color.Orange
                    );

                if (proj != default) Main.NewText(
                        target.FullName + " was hit by " + Player.name + " with " + proj.Name,
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
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
            => OnHitNPCWithAnything(target, damage, knockback, crit, item: item);

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
            => OnHitNPCWithAnything(target, crit ? damage * 2 : damage, knockback, crit, proj: proj);
    }
}
