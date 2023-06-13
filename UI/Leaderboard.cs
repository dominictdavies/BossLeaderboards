using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;

namespace Leaderboards.UI
{
    public class Leaderboard : UIState
    {
        private const float masterWidth = 600f;
        private const float masterHeight = 300f;
        private const float leaderWidth = masterWidth - 20f;
        private const float leaderHeight = masterHeight - 80f;
        private const int columns = 6;
        private UIPanel leaderPanel;
        private Dictionary<string, UIList> leaderColumns = new();

        public override void OnInitialize()
        {
            DragableUIPanel masterPanel = new DragableUIPanel();
            masterPanel.Width.Set(masterWidth, 0);
            masterPanel.Height.Set(masterHeight, 0);
            masterPanel.HAlign = masterPanel.VAlign = 0.5f;
            Append(masterPanel);

            UIText title = new UIText("Leaderboard");
            title.HAlign = 0.5f;
            title.Top.Set(15, 0);
            masterPanel.Append(title);

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
            leaderPanel.Width.Set(leaderWidth, 0);
            leaderPanel.Height.Set(leaderHeight, 0);
            leaderPanel.Top.Set(55, 0);
            leaderPanel.HAlign = 0.5f;
            masterPanel.Append(leaderPanel);
        }

        private void OnCloseButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            ModContent.GetInstance<LeaderboardSystem>().HideMyUI();
            SoundEngine.PlaySound(SoundID.MenuClose);
        }

        public void AddContribution(int whoAmI, Contribution contribution)
        {
            AddCell(leaderColumns["Name"], Main.player[whoAmI].name);
            AddCell(leaderColumns["Damage"], contribution.damage.ToString());
            AddCell(leaderColumns["Kills"], contribution.kills.ToString());
            AddCell(leaderColumns["Life Lost"], contribution.lifeLost.ToString());
            AddCell(leaderColumns["Hits Taken"], contribution.hitsTaken.ToString());
            AddCell(leaderColumns["Deaths"], contribution.deaths.ToString());
        }

        public void Clear()
        {
            leaderPanel.RemoveAllChildren();
            leaderColumns.Clear();
            AddColumns();
        }

        private void AddColumns()
        {
            AddColumn("Name");
            AddColumn("Damage");
            AddColumn("Kills");
            AddColumn("Life Lost");
            AddColumn("Hits Taken");
            AddColumn("Deaths");
        }

        private void AddColumn(string heading)
        {
            UIList column = new UIList();
            column.Width.Set(leaderWidth / columns, 0);
            column.Height.Set(leaderHeight, 0);
            column.HAlign = 1f / (float)(columns - 1) * (float)leaderColumns.Count;
            leaderPanel.Append(column);
            AddCell(column, heading);
            leaderColumns.Add(heading, column);
        }

        private void AddCell(UIList column, string text)
        {
            UIText cell = new UIText(text);
            cell.HAlign = 0.5f;
            column.Add(cell);
        }
    }
}
