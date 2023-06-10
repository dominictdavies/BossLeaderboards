using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.Audio;
using Terraria.ID;

namespace Leaderboards.UI
{
    public class Leaderboard : UIState
    {
        public override void OnInitialize()
        {
            UIPanel panel = new UIPanel();
            panel.Width.Set(300, 0);
            panel.Height.Set(300, 0);
            panel.HAlign = panel.VAlign = 0.5f;
            Append(panel);

            UIText title = new UIText("Leaderboard");
            title.HAlign = 0.5f;
            title.Top.Set(15, 0);
            panel.Append(title);

            UIPanel resetButton = new UIPanel();
            resetButton.Width.Set(30, 0);
            resetButton.Height.Set(30, 0);
            resetButton.Top.Set(10, 0);
            resetButton.Left.Set(10, 0);
            resetButton.OnClick += OnCloseButtonClick;
            panel.Append(resetButton);

            UIText resetText = new UIText("V");
            resetText.HAlign = resetText.VAlign = 0.5f;
            resetButton.Append(resetText);

            UIPanel closeButton = new UIPanel();
            closeButton.Width.Set(30, 0);
            closeButton.Height.Set(30, 0);
            closeButton.Top.Set(10, 0);
            closeButton.Left.Set(-10 - closeButton.Width.Pixels, 1);
            closeButton.OnClick += OnCloseButtonClick;
            panel.Append(closeButton);

            UIText closeText = new UIText("X");
            closeText.HAlign = closeText.VAlign = 0.5f;
            closeButton.Append(closeText);
        }

        private void OnCloseButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            ModContent.GetInstance<LeaderboardSystem>().HideMyUI();
            SoundEngine.PlaySound(SoundID.MenuTick);
        }
    }
}
