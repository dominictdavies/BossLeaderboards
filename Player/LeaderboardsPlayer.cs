using Leaderboards.UI;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Leaderboards
{
    internal partial class LeaderboardsPlayer : ModPlayer
    {
        private int targetOldLife;
        private int playerOldLife;
        public Dictionary<string, long> contribution = new() {
            { "Damage", 0 },
            { "Kills", 0 },
            { "Life Lost", 0 },
            { "Hits Taken", 0 },
            { "Deaths", 0 }
        };

        public override void OnEnterWorld(Player player)
        {
            UILeaderboardSystem leaderboardSystem = ModContent.GetInstance<UILeaderboardSystem>();
            UILeaderboard leaderboard = leaderboardSystem.leaderboard;
            leaderboardSystem.ShowMyUI();
            leaderboard.AddPlayer(player.whoAmI);
        }

        public override void PlayerConnect(Player player)
        {
            // Update other players' UI
        }

        public override void PlayerDisconnect(Player player)
        {
            if (Main.dedServ)
                return;

            UILeaderboardSystem leaderboardSystem = ModContent.GetInstance<UILeaderboardSystem>();
            UILeaderboard leaderboard = leaderboardSystem.leaderboard;
            leaderboard.RemovePlayer(player.whoAmI);
        }

        private void UpdateContribution(string statName, int value)
        {
            contribution[statName] += value;
            UILeaderboard leaderboard = ModContent.GetInstance<UILeaderboardSystem>().leaderboard;
            leaderboard.UpdateCell(Player.whoAmI, statName, contribution[statName]);
        }

        public void PreHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = null, Projectile proj = null)
            => targetOldLife = target.life;

        public void PostHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = null, Projectile proj = null)
        {
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            int damageDealt = target.life > 0 ? targetOldLife - target.life : targetOldLife;
            if (damageDealt > 0)
                UpdateContribution("Damage", damageDealt);

            if (target.life <= 0 && targetOldLife > 0)
                UpdateContribution("Kills", 1);
        }

        public void PreHitByAnything(int damage, bool crit, NPC npc = null, Projectile proj = null)
            => playerOldLife = Player.statLife;

        public void PostHitByAnything(int damage, bool crit, NPC npc = null, Projectile proj = null)
        {
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            int lifeLost = Player.statLife > 0 ? playerOldLife - Player.statLife : playerOldLife;
            if (lifeLost > 0)
            {
                UpdateContribution("Life Lost", lifeLost);
                UpdateContribution("Hits Taken", 1);
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            UpdateContribution("Deaths", 1);
        }
    }
}
