using Leaderboards.UI;
using Leaderboards.Keybinds;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace ExampleMod.Common.Players
{
    public class ExampleKeybindPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybindSystem.ShowLeaderboardKeybind.JustPressed)
            {
                LeaderboardSystem leaderboardSystem = ModContent.GetInstance<LeaderboardSystem>();
                if (leaderboardSystem.leaderboardInterface.CurrentState == null)
                    leaderboardSystem.ShowMyUI();
                else
                    leaderboardSystem.HideMyUI();
            }
        }
    }
}
