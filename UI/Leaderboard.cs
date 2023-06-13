using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.Audio;
using Terraria.ID;

namespace Leaderboards.UI
{
    public class Leaderboard : UIState
    {
        private UIPanel masterPanel;
        private UIText title;
        private UIPanel closeButton;
        private UIText closeText;
        private UIPanel leaderPanel;

        public override void OnInitialize()
        {
            masterPanel = new UIPanel();
            masterPanel.Width.Set(300, 0);
            masterPanel.Height.Set(300, 0);
            masterPanel.HAlign = masterPanel.VAlign = 0.5f;
            Append(masterPanel);

            title = new UIText("Leaderboard");
            title.HAlign = 0.5f;
            title.Top.Set(15, 0);
            masterPanel.Append(title);

            // UIPanel resetButton = new UIPanel();
            // resetButton.Width.Set(30, 0);
            // resetButton.Height.Set(30, 0);
            // resetButton.Top.Set(10, 0);
            // resetButton.Left.Set(10, 0);
            // resetButton.OnClick += OnResetButtonClick;
            // masterPanel.Append(resetButton);

            // UIText resetText = new UIText("V");
            // resetText.HAlign = resetText.VAlign = 0.5f;
            // resetButton.Append(resetText);

            closeButton = new UIPanel();
            closeButton.Width.Set(30, 0);
            closeButton.Height.Set(30, 0);
            closeButton.Top.Set(10, 0);
            closeButton.Left.Set(-10 - closeButton.Width.Pixels, 1);
            closeButton.OnClick += OnCloseButtonClick;
            masterPanel.Append(closeButton);

            closeText = new UIText("X");
            closeText.HAlign = closeText.VAlign = 0.5f;
            closeButton.Append(closeText);

            leaderPanel = new UIPanel();
            leaderPanel.Width.Set(280, 0);
            leaderPanel.Height.Set(220, 0);
            leaderPanel.Top.Set(55, 0);
            leaderPanel.HAlign = 0.5f;
            masterPanel.Append(leaderPanel);
        }

        // private void OnResetButtonClick(UIMouseEvent evt, UIElement listeningElement)
        // {
        //     leaderPanel.RemoveAllChildren();

        //     UIList nameList = new UIList();
        //     nameList.Width.Set(280 / 3, 0);
        //     nameList.Height.Set(220, 0);
        //     nameList.HAlign = 0f;
        //     leaderPanel.Append(nameList);
        //     UIText headingName = new UIText("Name");
        //     headingName.HAlign = 0.5f;
        //     nameList.Add(headingName);

        //     UIList damageList = new UIList();
        //     damageList.Width.Set(280 / 3, 0);
        //     damageList.Height.Set(220, 0);
        //     damageList.HAlign = 0.5f;
        //     leaderPanel.Append(damageList);
        //     UIText headingDamage = new UIText("Damage");
        //     headingDamage.HAlign = 0.5f;
        //     damageList.Add(headingDamage);

        //     UIList lifeLostList = new UIList();
        //     lifeLostList.Width.Set(280 / 3, 0);
        //     lifeLostList.Height.Set(220, 0);
        //     lifeLostList.HAlign = 1f;
        //     leaderPanel.Append(lifeLostList);
        //     UIText headingLifeLost = new UIText("Life Lost");
        //     headingLifeLost.HAlign = 0.5f;
        //     lifeLostList.Add(headingLifeLost);

        //     for (int i = 0; i < Main.player.Length; i++)
        //     {
        //         Player player = Main.player[i];
        //         if (player.active)
        //         {
        //             UIText name = new UIText(player.name);
        //             name.HAlign = 0.5f;
        //             nameList.Add(name);

        //             UIText damage = new UIText(player.statDefense.ToString());
        //             damage.HAlign = 0.5f;
        //             damageList.Add(damage);

        //             UIText lifeLost = new UIText(player.statLife.ToString());
        //             lifeLost.HAlign = 0.5f;
        //             lifeLostList.Add(lifeLost);
        //         }
        //     }

        //     SoundEngine.PlaySound(SoundID.Chat);
        // }

        private void OnCloseButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            ModContent.GetInstance<LeaderboardSystem>().HideMyUI();
            SoundEngine.PlaySound(SoundID.MenuClose);
        }
    }
}
