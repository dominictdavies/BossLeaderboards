using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class LeaderboardsNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            Leaderboards.NewMessage(npc.FullName + " was killed!");
        }
    }
}
