using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Leaderboards
{
    public partial class LeaderboardsPlayer : ModPlayer
    {
        public Dictionary<string, Contribution> bossContributions = new();
        public int targetOldLife;
        public int playerOldLife;

        public override void PreUpdate()
        {
            // One execution after all active bosses are defeated
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC && bossContributions.Count > 0) {
                if (Main.netMode == NetmodeID.MultiplayerClient) {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write(bossContributions.Count); // Write the number of contributions

                    foreach (KeyValuePair<string, Contribution> bossContribution in bossContributions) {
                        packet.Write(bossContribution.Key);
                        packet.Write(bossContribution.Value.totalDamage);
                        packet.Write(bossContribution.Value.totalLifeLost);
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
                    if (bossContributions.TryGetValue(target.FullName, out Contribution contribution)) {
                        contribution.totalDamage += damageDealt;
                    } else {
                        bossContributions.Add(target.FullName, new Contribution(totalDamage: damageDealt));
                    }
                }
            }
        }

        public void ModifyHitByAnything(int damage, bool crit, NPC npc = null, Projectile proj = null)
            => playerOldLife = Player.statLife;

        public void OnHitByAnything(int damage, bool crit, NPC npc = null, Projectile proj = null)
        {
            npc ??= Main.npc[proj.owner];

            if (Main.CurrentFrameFlags.AnyActiveBossNPC) {
                int lifeLost = Player.statLife > 0 ? playerOldLife - Player.statLife : playerOldLife;
                if (lifeLost > 0) {
                    if (bossContributions.TryGetValue(npc.FullName, out Contribution contribution)) {
                        contribution.totalLifeLost += lifeLost;
                    } else {
                        bossContributions.Add(npc.FullName, new Contribution(totalLifeLost: lifeLost));
                    }
                }
            }
        }
    }
}
