using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;

namespace Leaderboards.UI
{
    internal class UILeaderboard : UIState
    {
        private const float masterWidth = 600f;
        private const float masterHeight = 300f;
        private const float leaderWidth = masterWidth - 20f;
        private const float leaderHeight = masterHeight - 80f;
        private const int columns = 6;
        private UIPanel leaderPanel;
        private Dictionary<UIList, List<UIText>> columnCells = new();

        public override void OnInitialize()
        {
            UIDragablePanel masterPanel = new UIDragablePanel();
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
            ModContent.GetInstance<UILeaderboardSystem>().HideMyUI();
            SoundEngine.PlaySound(SoundID.MenuClose);
        }

        public void PushContribution(int whoAmI)
        {
            if (columnCells.Count == 0)
                AddColumns();

            Player player = Main.player[whoAmI];
            LeaderboardsPlayer leaderboardsPlayer = player.GetModPlayer<LeaderboardsPlayer>();
            Contribution contribution = leaderboardsPlayer.contribution;
            foreach (KeyValuePair<UIList, List<UIText>> pair in columnCells)
            {
                UIList column = pair.Key;
                List<UIText> cells = pair.Value;

                switch (cells[0].Text)
                {
                    case "Name":
                        cells[1].SetText(player.name);
                        break;
                    case "Damage":
                        cells[1].SetText(contribution.damage.ToString());
                        break;
                    case "Kills":
                        cells[1].SetText(contribution.kills.ToString());
                        break;
                    case "Life Lost":
                        cells[1].SetText(contribution.lifeLost.ToString());
                        break;
                    case "Hits Taken":
                        cells[1].SetText(contribution.hitsTaken.ToString());
                        break;
                    case "Deaths":
                        cells[1].SetText(contribution.deaths.ToString());
                        break;
                }
            }
        }

        private void AddColumns()
        {
            AddColumn("Name");
            AddColumn("Damage");
            AddColumn("Kills");
            AddColumn("Life Lost");
            AddColumn("Hits Taken");
            AddColumn("Deaths");

            foreach (Player player in Main.player)
            {
                if (!player.active)
                    continue;

                LeaderboardsPlayer leaderboardsPlayer = player.GetModPlayer<LeaderboardsPlayer>();
                Contribution contribution = leaderboardsPlayer.contribution;

                foreach (KeyValuePair<UIList, List<UIText>> pair in columnCells)
                {
                    UIList column = pair.Key;
                    List<UIText> cells = pair.Value;

                    switch (cells[0].Text)
                    {
                        case "Name":
                            AddCell(column, player.name);
                            break;
                        case "Damage":
                            AddCell(column, contribution.damage.ToString());
                            break;
                        case "Kills":
                            AddCell(column, contribution.kills.ToString());
                            break;
                        case "Life Lost":
                            AddCell(column, contribution.lifeLost.ToString());
                            break;
                        case "Hits Taken":
                            AddCell(column, contribution.hitsTaken.ToString());
                            break;
                        case "Deaths":
                            AddCell(column, contribution.deaths.ToString());
                            break;
                    }
                }
            }
        }

        private void AddColumn(string heading)
        {
            UIList column = new UIList();
            column.Width.Set(leaderWidth / columns, 0);
            column.Height.Set(leaderHeight, 0);
            column.HAlign = 1f / (float)(columns - 1) * (float)columnCells.Count;
            leaderPanel.Append(column);

            UIText headingText = new UIText(heading);
            headingText.HAlign = 0.5f;
            column.Add(headingText);
            columnCells.Add(column, new());
        }

        private void AddCell(UIList column, string text)
        {
            UIText cell = new UIText(text);
            cell.HAlign = 0.5f;
            column.Add(cell);
            columnCells[column].Add(cell);
        }
    }
}
