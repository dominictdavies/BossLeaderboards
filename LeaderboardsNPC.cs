using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class LeaderboardsNPC : GlobalNPC
    {
        public void PlayersShare()
        {
            foreach (Player player in Main.player) {
                if (player.active) {
                    player.GetModPlayer<LeaderboardsPlayer>().share = true;
                }
            }
        }

        public override void OnKill(NPC npc) {
            if (npc.boss) PlayersShare();
        }
    }
}
