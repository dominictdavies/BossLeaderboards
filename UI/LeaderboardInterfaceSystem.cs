using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Audio;

namespace BossLeaderboards.UI
{
    internal partial class LeaderboardSystem : ModSystem
    {
        internal UserInterface leaderboardInterface;
        internal UILeaderboard leaderboard;
        private GameTime _oldUpdateUiGameTime;

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

        internal void ShowMyUI(bool playSound = true)
        {
            leaderboardInterface?.SetState(leaderboard);
            if (playSound)
                SoundEngine.PlaySound(SoundID.MenuOpen);
        }

        internal void HideMyUI(bool playSound = true)
        {
            leaderboardInterface?.SetState(null);
            if (playSound)
                SoundEngine.PlaySound(SoundID.MenuClose);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex == -1)
                return;

            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "BossLeaderboards: LeaderboardInterface",
                delegate
                {
                    if (_oldUpdateUiGameTime != null && leaderboardInterface?.CurrentState != null)
                        leaderboardInterface.Draw(Main.spriteBatch, _oldUpdateUiGameTime);
                    return true;
                },
                InterfaceScaleType.UI
                )
            );
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (leaderboardInterface?.CurrentState != null)
                leaderboardInterface.Update(gameTime);
            _oldUpdateUiGameTime = gameTime;
        }
    }
}
