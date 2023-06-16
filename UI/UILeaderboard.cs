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
        private const float _masterWidth = 600f;
        private const float _masterHeight = 300f;
        private const float _leaderWidth = _masterWidth - 20f;
        private const float _leaderHeight = _masterHeight - 80f;
        private UIPanel _leaderPanel;
        private List<UIList> _columns = new();
        private string[] _headings = { "Name", "Damage", "Kills", "Life Lost", "Hits Taken", "Deaths" };

        public override void OnInitialize()
        {
            UIDragablePanel masterPanel = new UIDragablePanel();
            masterPanel.Width.Set(_masterWidth, 0);
            masterPanel.Height.Set(_masterHeight, 0);
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

            _leaderPanel = new UIPanel();
            _leaderPanel.Width.Set(_leaderWidth, 0);
            _leaderPanel.Height.Set(_leaderHeight, 0);
            _leaderPanel.Top.Set(55, 0);
            _leaderPanel.HAlign = 0.5f;
            masterPanel.Append(_leaderPanel);
        }

        private void OnCloseButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            ModContent.GetInstance<UILeaderboardSystem>().HideMyUI();
            SoundEngine.PlaySound(SoundID.MenuClose);
        }

        public void UpdateCells()
        {
            if (_columns.Count == 0)
                AddColumns();
            else
                ClearColumns();

            foreach (Player player in Main.player)
            {
                if (!player.active)
                    continue;

                LeaderboardsPlayer leaderboardsPlayer = player.GetModPlayer<LeaderboardsPlayer>();
                Contribution contribution = leaderboardsPlayer.contribution;

                foreach (UIList column in _columns)
                {
                    switch (((UIText)column._items[0]).Text)
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

        private void ClearColumns()
        {
            foreach (UIList column in _columns)
                for (int i = column.Count - 1; i > 0; i--)
                    column.Remove(column._items[i]);
        }

        private void AddColumns()
        {
            foreach (string heading in _headings)
                AddColumn(heading);
        }

        private void AddColumn(string heading)
        {
            UIList column = new UIList();
            column.Width.Set(_leaderWidth / _headings.Length, 0);
            column.Height.Set(_leaderHeight, 0);
            column.HAlign = 1f / (float)(_headings.Length - 1) * (float)_columns.Count;
            _leaderPanel.Append(column);

            UIText headingText = new UIText(heading);
            headingText.HAlign = 0.5f;
            column.Add(headingText);
            _columns.Add(column);
        }

        private void AddCell(UIList column, string text)
        {
            UIText cell = new UIText(text);
            cell.HAlign = 0.5f;
            column.Add(cell);
        }
    }
}
