using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;

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

        private const int PacketTimerMax = 60;
        private int _packetTimer = 0;
        private GameTime _lastUpdateUiGameTime;
        private bool _lastAnyActiveBossNPC;

        public override void UpdateUI(GameTime gameTime)
        {
            if (leaderboardInterface?.CurrentState != null)
                leaderboardInterface.Update(gameTime);

            if (Main.CurrentFrameFlags.AnyActiveBossNPC)
            {
                Player player = Main.player[Main.myPlayer];
                LeaderboardsPlayer leaderboardsPlayer = player.GetModPlayer<LeaderboardsPlayer>();

                if (!_lastAnyActiveBossNPC) // Boss just spawned
                    leaderboardsPlayer.contribution = new Contribution();

                UILeaderboardSystem leaderboardSystem = ModContent.GetInstance<UILeaderboardSystem>();
                UILeaderboard leaderboard = leaderboardSystem.leaderboard;
                leaderboard.FillCells();

                if (Main.netMode == NetmodeID.MultiplayerClient && _packetTimer-- == 0)
                {
                    SendContribution();
                    _packetTimer = PacketTimerMax;
                }
            }
            else if (_lastAnyActiveBossNPC) // Boss just died
            {
                SendContribution();
                UILeaderboardSystem leaderboardSystem = ModContent.GetInstance<UILeaderboardSystem>();
                leaderboardSystem.ShowMyUI();
            }

            _lastUpdateUiGameTime = gameTime;
            _lastAnyActiveBossNPC = Main.CurrentFrameFlags.AnyActiveBossNPC;
        }

        private void SendContribution()
        {
            Player player = Main.player[Main.myPlayer];
            LeaderboardsPlayer leaderboardsPlayer = player.GetModPlayer<LeaderboardsPlayer>();
            Contribution contribution = leaderboardsPlayer.contribution;
            ModPacket packet = Mod.GetPacket();
            packet.Write(contribution.damage);
            packet.Write(contribution.kills);
            packet.Write(contribution.lifeLost);
            packet.Write(contribution.hitsTaken);
            packet.Write(contribution.deaths);
            packet.Send();
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
