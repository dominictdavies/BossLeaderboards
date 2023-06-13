using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Leaderboards.UI
{
    internal class UILeaderboardSystem : ModSystem
    {
        internal UserInterface leaderboardInterface;
        internal UILeaderboard leaderboard;

        internal void ShowMyUI()
        {
            leaderboardInterface?.SetState(leaderboard);
        }

        internal void HideMyUI()
        {
            leaderboardInterface?.SetState(null);
        }

        public override void Load()
        {
            if (Main.dedServ)
                return;

            leaderboardInterface = new UserInterface();
            leaderboard = new UILeaderboard();
            leaderboard.Activate();
        }

        public override void Unload()
        {
            leaderboard = null;
        }

        private GameTime _lastUpdateUiGameTime;

        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (leaderboardInterface?.CurrentState != null)
                leaderboardInterface.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex == -1)
                return;

            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "Leaderboards: LeaderboardInterface",
                delegate
                {
                    if (_lastUpdateUiGameTime != null && leaderboardInterface?.CurrentState != null)
                        leaderboardInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                    return true;
                },
                InterfaceScaleType.UI
                )
            );
        }
    }
}
