using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class LeaderboardsNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private bool despawning = false;

        public void ShareContributions(NPC npc)
        {
            foreach (Player player in Main.player) {
                if (player.active) {
                    player.GetModPlayer<LeaderboardsPlayer>().shareContributions.Add(npc.whoAmI);
                }
            }
        }

        public override void ResetEffects(NPC npc)
        {
            if (npc.despawnEncouraged && !despawning) {
                ShareContributions(npc);
                despawning = true;
            }
        }

        public override void OnKill(NPC npc)
        {
            ShareContributions(npc);
        }
    }
}
