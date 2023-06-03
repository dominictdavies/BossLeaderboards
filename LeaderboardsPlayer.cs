using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Leaderboards
{
    public partial class LeaderboardsPlayer : ModPlayer
    {
        public Dictionary<string, Contribution> contributions;
        public int targetOldLife;
        public int playerOldLife;

        public override void OnEnterWorld(Player player)
            => contributions = new();

        public void PreHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = null, Projectile proj = null)
            => targetOldLife = target.life;

        public void PostHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = null, Projectile proj = null)
        {
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            int damageDealt = target.life > 0 ? targetOldLife - target.life : targetOldLife;
            if (damageDealt > 0) {
                if (contributions.TryGetValue(target.FullName, out Contribution contribution)) {
                    contribution.totalDamageTo += damageDealt;
                } else {
                    contributions.Add(target.FullName, new Contribution(totalDamageTo: damageDealt));
                }
            }
        }

        public void PreHitByAnything(int damage, bool crit, NPC npc = null, Projectile proj = null)
            => playerOldLife = Player.statLife;

        public void PostHitByAnything(int damage, bool crit, NPC npc = null, Projectile proj = null)
        {
            npc ??= Main.npc[proj.owner]; // What if no owner?

            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            int lifeLost = Player.statLife > 0 ? playerOldLife - Player.statLife : playerOldLife;
            if (lifeLost > 0) {
                if (contributions.TryGetValue(npc.FullName, out Contribution contribution)) {
                    contribution.totalLifeLostFrom += lifeLost;
                } else {
                    contributions.Add(npc.FullName, new Contribution(totalLifeLostFrom: lifeLost));
                }
            }
        }

        public override void PreUpdate()
        {
            if (Main.CurrentFrameFlags.AnyActiveBossNPC || contributions.Count == 0)
                return; // Proceed in execution if client participated in boss battle

            if (Main.netMode == NetmodeID.MultiplayerClient) {
                ModPacket packet = Mod.GetPacket();
                packet.Write(contributions.Count);

                foreach (KeyValuePair<string, Contribution> bossContribution in contributions) {
                    packet.Write(bossContribution.Key);
                    packet.Write(bossContribution.Value.totalDamageTo);
                    packet.Write(bossContribution.Value.totalLifeLostFrom);
                }

                packet.Send();
            }

            LeaderboardsFunctions.PushContribution(Player);
        }
    }
}
