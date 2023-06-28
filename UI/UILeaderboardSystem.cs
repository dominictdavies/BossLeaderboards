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

        internal void ShowMyUI()
        {
            leaderboardInterface?.SetState(leaderboard);
        }

        internal void HideMyUI()
        {
            leaderboardInterface?.SetState(null);
        }

        private const int PacketTimerMax = 15;
        private int _packetTimer = PacketTimerMax;
        private bool _lastAnyActiveBossNPC = false;
        private GameTime _lastUpdateUiGameTime;

        public override void UpdateUI(GameTime gameTime)
        {
            if (Main.CurrentFrameFlags.AnyActiveBossNPC)
            {
                if (!_lastAnyActiveBossNPC) // Boss just spawned
                {
                    leaderboard.RemoveData();
                    SetNewContributions();
                }

                if (Main.netMode == NetmodeID.MultiplayerClient && _packetTimer-- == 0)
                {
                    SendContribution();
                    _packetTimer = PacketTimerMax;
                }
            }
            else if (_lastAnyActiveBossNPC) // Boss just died
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    SendContribution(force: true);
            }

            if (leaderboardInterface?.CurrentState != null)
                leaderboardInterface.Update(gameTime);

            _lastAnyActiveBossNPC = Main.CurrentFrameFlags.AnyActiveBossNPC;
            _lastUpdateUiGameTime = gameTime;
        }

        private void SetNewContributions()
        {
            foreach (Player player in Utilities.GetActivePlayers())
            {
                LeaderboardsPlayer leaderboardsPlayer = player.GetModPlayer<LeaderboardsPlayer>();
                leaderboardsPlayer.contribution = new Contribution(player.whoAmI);
                leaderboardsPlayer.contribution.AddToLeaderboard();
            }
        }

        private void SendContribution(bool force = false)
        {
            LeaderboardsPlayer localLeaderboardsPlayer = Main.LocalPlayer.GetModPlayer<LeaderboardsPlayer>();
            Contribution contribution = localLeaderboardsPlayer.contribution;
            ModPacket packet = Mod.GetPacket();
            packet.Write(force ? true : false);
            packet.Write((long)contribution.GetStat("Damage"));
            packet.Write((long)contribution.GetStat("Kills"));
            packet.Write((long)contribution.GetStat("Life Lost"));
            packet.Write((long)contribution.GetStat("Hits Taken"));
            packet.Write((long)contribution.GetStat("Deaths"));
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
