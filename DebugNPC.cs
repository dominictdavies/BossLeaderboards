// Debug file

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class DebugNPC : GlobalNPC
    {
        public override void SetDefaults(NPC npc)
        {
            if (npc.type == NPCID.SkeletronHead) npc.aiStyle = 0;
        }
    }
}
