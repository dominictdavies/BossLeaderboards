using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Leaderboards.UI
{
    public class UILeaderboardSystem : ModSystem
    {
        internal UserInterface LeaderboardInterface;
        internal UILeaderboard leaderboard;

        internal void ShowMyUI()
        {
            LeaderboardInterface?.SetState(leaderboard);
        }

        internal void HideMyUI()
        {
            LeaderboardInterface?.SetState(null);
        }

        public override void Load()
        {
            if (Main.dedServ)
                return;

            LeaderboardInterface = new UserInterface();

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
            if (LeaderboardInterface?.CurrentState != null)
            {
                LeaderboardInterface.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "MyMod: MyInterface",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && LeaderboardInterface?.CurrentState != null)
                        {
                            LeaderboardInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI
                    )
                );
            }
        }
    }
}
