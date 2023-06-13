using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Leaderboards
{
    public partial class LeaderboardsPlayer : ModPlayer
    {
        public Contribution contribution = new();
        public bool oldAnyActiveBossNPC;
        public int targetOldLife;
        public int playerOldLife;

        public void PreHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = null, Projectile proj = null)
            => targetOldLife = target.life;

        public void PostHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, Item item = null, Projectile proj = null)
        {
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            int damageDealt = target.life > 0 ? targetOldLife - target.life : targetOldLife;
            if (damageDealt > 0)
                contribution.damage += damageDealt;

            if (target.life <= 0 && targetOldLife > 0)
                contribution.kills++;
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
                contribution.lifeLost += lifeLost;
                contribution.hitsTaken++;
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (!Main.CurrentFrameFlags.AnyActiveBossNPC)
                return;

            contribution.deaths++;
        }

        public override void PreUpdate()
        {
            if (Player.whoAmI != Main.myPlayer || contribution.IsEmpty() || Main.CurrentFrameFlags.AnyActiveBossNPC)
                return; // Proceed if my client, I have a contribution, no boss active this frame

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write(contribution.damage);
                packet.Write(contribution.kills);
                packet.Write(contribution.lifeLost);
                packet.Write(contribution.hitsTaken);
                packet.Write(contribution.deaths);
                packet.Send();
            }

            LeaderboardsFunctions.PushContribution(Player);
        }

        public override void PostUpdate()
        {
            oldAnyActiveBossNPC = Main.CurrentFrameFlags.AnyActiveBossNPC;
        }
    }
}
