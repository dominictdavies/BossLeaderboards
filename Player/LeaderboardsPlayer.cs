using Leaderboards.UI;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Leaderboards
{
    public partial class LeaderboardsPlayer : ModPlayer
    {
        public Dictionary<string, Contribution> contributions = new();
        public int targetOldLife;
        public int playerOldLife;

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
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            string fullName = npc != null ? npc.FullName : proj.Name;
            int lifeLost = Player.statLife > 0 ? playerOldLife - Player.statLife : playerOldLife;
            if (lifeLost > 0) {
                if (contributions.TryGetValue(fullName, out Contribution contribution))
                    contribution.totalLifeLostFrom += lifeLost;
                else
                    contributions.Add(fullName, new Contribution(totalLifeLostFrom: lifeLost));
            }
        }

        public override void PreUpdate()
        {
            if (Main.CurrentFrameFlags.AnyActiveBossNPC || contributions.Count == 0)
                return; // Proceed in execution if client participated in boss battle

            ModContent.GetInstance<LeaderboardSystem>().ShowMyUI();

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
