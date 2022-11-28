using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class LeaderboardsNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            foreach(Player player in Main.player) {
                if (player.active) {
                    player.GetModPlayer<LeaderboardsPlayer>().shareContributions.Add(npc.whoAmI);
                }
            }
        }
    }
}
