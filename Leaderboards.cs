using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class Leaderboards : Mod
    {
        public static void NewMessage(string text, Color color = default)
        {
            if (color == default) color = Color.White;

            if (Main.netMode == NetmodeID.SinglePlayer) {
                Main.NewText(text, color);
            } else {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
            }
        }
    }
}
