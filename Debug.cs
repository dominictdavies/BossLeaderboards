using Terraria.ID;

namespace Leaderboards
{
    public static class Debug
    {
        public readonly static bool chat;
        public readonly static int[] disableAI;

        static Debug()
        {
            chat = false;
            disableAI = new int[] { NPCID.SkeletronHead };
        }
    }
}
