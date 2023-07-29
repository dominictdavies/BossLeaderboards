using Leaderboards.UI;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Leaderboards
{
    internal partial class LeaderboardsPlayer : ModPlayer
    {
        private bool _bossDeath;
        private bool _keepUIShown;
        public Contribution contribution = new Contribution();

        public override void OnEnterWorld()
        {
            UILeaderboard leaderboard = ModContent.GetInstance<LeaderboardSystem>().leaderboard;
            leaderboard.RemoveData(true);
        }

        public override void OnRespawn()
        {
            if (Player.whoAmI == Main.myPlayer && _bossDeath && !_keepUIShown)
                ModContent.GetInstance<LeaderboardSystem>().HideMyUI(playSound: false);

            _bossDeath = false;
            _keepUIShown = false;
        }

        public void OnHitNPCWithAnything(NPC target, NPC.HitInfo hit, int damageDone, Item item = null, Projectile proj = null)
        {
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            if (damageDone > 0)
                contribution.IncreaseStat(Player.whoAmI, "Damage", damageDone);

            if (target.life <= 0)
                contribution.IncreaseStat(Player.whoAmI, "Kills");
        }

        public void OnHitByAnything(Player.HurtInfo hurtInfo, NPC npc = null, Projectile proj = null)
        {
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            contribution.IncreaseStat(Player.whoAmI, "Life Lost", hurtInfo.Damage);
            contribution.IncreaseStat(Player.whoAmI, "Hits Taken");
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            if (Player.whoAmI == Main.myPlayer)
            {
                LeaderboardSystem leaderboardSystem = ModContent.GetInstance<LeaderboardSystem>();

                _bossDeath = true;
                if (leaderboardSystem.leaderboardInterface.CurrentState != null)
                    _keepUIShown = true;

                leaderboardSystem.ShowMyUI(playSound: false);
            }

            contribution.IncreaseStat(Player.whoAmI, "Deaths");
        }
    }
}
