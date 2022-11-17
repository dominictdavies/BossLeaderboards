using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Leaderboards
{
    public class LeaderboardsNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            foreach (Player player in Main.player) {
                if (player.active) {
                    Dictionary<int, int> BossDamages = player.GetModPlayer<LeaderboardsPlayer>().BossDamages;
                    if (BossDamages.ContainsKey(npc.whoAmI)) {
                        Leaderboards.NewMessage(
                            player.name + " dealt " + BossDamages[npc.whoAmI].ToString() + " damage.",
                            Color.Yellow
                        );
                        BossDamages.Remove(npc.whoAmI);
                    }
                }
            }
        }
    }
}
