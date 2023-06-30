using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Leaderboards.UI
{
    internal partial class LeaderboardSystem : ModSystem
    {
        private const int PacketTimerMax = 15;
        private int _packetTimer = PacketTimerMax;
        private bool _oldAnyActiveBossNPC = false;

        public override void UpdateUI(GameTime gameTime)
        {
            if (Main.CurrentFrameFlags.AnyActiveBossNPC)
            {
                if (!_oldAnyActiveBossNPC) // Boss just spawned
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
            else if (_oldAnyActiveBossNPC) // Boss just died
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    SendContribution();
            }

            if (leaderboardInterface?.CurrentState != null)
                leaderboardInterface.Update(gameTime);

            _oldAnyActiveBossNPC = Main.CurrentFrameFlags.AnyActiveBossNPC;
            _lastUpdateUiGameTime = gameTime;
        }

        private void SetNewContributions()
        {
            foreach (Player player in Utilities.GetActivePlayers())
            {
                LeaderboardsPlayer leaderboardsPlayer = player.GetModPlayer<LeaderboardsPlayer>();
                leaderboardsPlayer.contribution = new Contribution();
                ModContent.GetInstance<LeaderboardSystem>().leaderboard.AddPlayer(player.whoAmI, leaderboardsPlayer.contribution);
            }
        }

        private void SendContribution()
        {
            LeaderboardsPlayer localLeaderboardsPlayer = Main.LocalPlayer.GetModPlayer<LeaderboardsPlayer>();
            Contribution contribution = localLeaderboardsPlayer.contribution;
            ModPacket packet = Mod.GetPacket();
            foreach (string statName in Contribution.StatNames)
                packet.Write((long)contribution.GetStat(statName));
            packet.Send();
        }
    }
}
