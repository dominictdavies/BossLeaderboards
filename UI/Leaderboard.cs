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

            UIText header = new UIText("Leaderboard");
            header.HAlign = 0.5f;
            header.Top.Set(15, 0);
            panel.Append(header);

            UIPanel button = new UIPanel();
            button.Width.Set(30, 0);
            button.Height.Set(30, 0);
            button.Top.Set(10, 0);
            button.Left.Set(10, 0);
            button.OnClick += OnButtonClick;
            panel.Append(button);

            UIText text = new UIText("X");
            text.HAlign = text.VAlign = 0.5f;
            button.Append(text);
        }

        private void OnButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            ModContent.GetInstance<LeaderboardSystem>().HideMyUI();
            SoundEngine.PlaySound(SoundID.MenuTick);
        }
    }
}
