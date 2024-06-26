using BossLeaderboards.UI;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace BossLeaderboards.Source.Common.Player
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
