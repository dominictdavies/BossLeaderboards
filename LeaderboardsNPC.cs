using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class LeaderboardsNPC : GlobalNPC
    {
        private bool despawnFlag = false;

        public override bool InstancePerEntity => true;

        public void ShareNPCContributions(NPC npc)
        {
            foreach (Player player in Main.player) {
                if (player.active) {
                    player.GetModPlayer<LeaderboardsPlayer>().shareContributions.Add(npc.whoAmI);
                }
            }
        }

        public override void OnKill(NPC npc)
        {
            ShareNPCContributions(npc);
        }

        public override void ResetEffects(NPC npc)
        {
            if (npc.despawnEncouraged && !despawnFlag) {
                ShareNPCContributions(npc);
                despawnFlag = true;
            }
        }
    }
}
