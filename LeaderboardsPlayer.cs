using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class LeaderboardsPlayer : ModPlayer
    {
        public Dictionary<string, Contribution> bossContributions = new();
        public int targetOldLife;

        public override void PreUpdate()
        {
            // One execution after all active bosses are defeated
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC && bossContributions.Count > 0) {
                if (Main.netMode == NetmodeID.MultiplayerClient) {
                    ModPacket packet = Mod.GetPacket();
                    foreach (KeyValuePair<string, Contribution> pair in bossContributions) {
                        packet.Write(pair.Key);
                        packet.Write(pair.Value.totalDamage);
                        packet.Write(pair.Value.totalLifeLost);
                    }
                    packet.Send();
                }

                LeaderboardsFunctions.PushContribution(Player);
            }
        }

        public void ModifyHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = null, Projectile proj = null)
            => targetOldLife = target.life;

        public void OnHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = null, Projectile proj = null)
        {
            if (Main.CurrentFrameFlags.AnyActiveBossNPC) {
                int damageDealt = target.life > 0 ? targetOldLife - target.life : targetOldLife;
                if (damageDealt > 0) {
                    if (bossContributions.ContainsKey(target.FullName)) {
                        bossContributions[target.FullName].totalDamage += damageDealt;
                    } else {
                        bossContributions[target.FullName] = new Contribution(damageDealt);
                    }
                }
            }
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
            => ModifyHitNPCWithAnything(target, damage, knockback, crit, item: item);

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
            => ModifyHitNPCWithAnything(target, damage, knockback, crit, proj: proj);

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
            => OnHitNPCWithAnything(target, damage, knockback, crit, item: item);

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
            => OnHitNPCWithAnything(target, damage, knockback, crit, proj: proj);
    }
}
