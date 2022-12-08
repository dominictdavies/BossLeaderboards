using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class LeaderboardsNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public int oldLife;

        public override void PostAI(NPC npc)
            => oldLife = npc.life;

        public override void SetDefaults(NPC npc)
        {
            if (Debug.disableAI.Contains(npc.type)) npc.aiStyle = 0;
        }
    }
}
