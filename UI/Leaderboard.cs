using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.Audio;
using Terraria.ID;

namespace Leaderboards.UI
{
    public class Leaderboard : UIState
    {
        private UIPanel leaderPanel;

        public override void OnInitialize()
        {
            UIPanel masterPanel = new UIPanel();
            masterPanel.Width.Set(300, 0);
            masterPanel.Height.Set(300, 0);
            masterPanel.HAlign = masterPanel.VAlign = 0.5f;
            Append(masterPanel);

            UIText title = new UIText("Leaderboard");
            title.HAlign = 0.5f;
            title.Top.Set(15, 0);
            masterPanel.Append(title);

            UIPanel resetButton = new UIPanel();
            resetButton.Width.Set(30, 0);
            resetButton.Height.Set(30, 0);
            resetButton.Top.Set(10, 0);
            resetButton.Left.Set(10, 0);
            resetButton.OnClick += OnResetButtonClick;
            masterPanel.Append(resetButton);

            UIText resetText = new UIText("V");
            resetText.HAlign = resetText.VAlign = 0.5f;
            resetButton.Append(resetText);

            UIPanel closeButton = new UIPanel();
            closeButton.Width.Set(30, 0);
            closeButton.Height.Set(30, 0);
            closeButton.Top.Set(10, 0);
            closeButton.Left.Set(-10 - closeButton.Width.Pixels, 1);
            closeButton.OnClick += OnCloseButtonClick;
            masterPanel.Append(closeButton);

            UIText closeText = new UIText("X");
            closeText.HAlign = closeText.VAlign = 0.5f;
            closeButton.Append(closeText);

            leaderPanel = new UIPanel();
            leaderPanel.Width.Set(280, 0);
            leaderPanel.Height.Set(220, 0);
            leaderPanel.Top.Set(55, 0);
            leaderPanel.HAlign = 0.5f;
            masterPanel.Append(leaderPanel);
        }

        private void OnResetButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            leaderPanel.RemoveAllChildren();
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                if (player.active)
                {
                    UIText playerText = new UIText(player.name);
                    playerText.HAlign = 0.5f;
                    playerText.Top.Set(i * 15, 0);
                    leaderPanel.Append(playerText);
                }
            }
            SoundEngine.PlaySound(SoundID.Chat);
        }

        private void OnCloseButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            ModContent.GetInstance<LeaderboardSystem>().HideMyUI();
            SoundEngine.PlaySound(SoundID.MenuClose);
        }
    }
}
