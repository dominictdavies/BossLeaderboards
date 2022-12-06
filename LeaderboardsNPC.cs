using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class LeaderboardsNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private bool despawning = false;

        public void PlayersShare()
        {
            foreach (Player player in Main.player) {
                if (player.active) {
                    player.GetModPlayer<LeaderboardsPlayer>().share = true;
                }
            }
        }

        public override void ResetEffects(NPC npc)
        {
            if (npc.despawnEncouraged && !despawning) {
                PlayersShare();
                despawning = true;
            }
        }

        public override void OnKill(NPC npc) => PlayersShare();
    }
}
