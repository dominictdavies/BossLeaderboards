using BossLeaderboards.Source.Common.Player;
using BossLeaderboards.UI;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace BossLeaderboards.Source.Common.UI
{
    internal class UILeaderboard : UIState
    {
        private const float MasterPanelWidth = 700f;
        private const float MasterPanelHeight = 350f;
        private const float StatPanelHeight = 40f;
        private const float PlayerPanelWidth = 120f;
        private const float CloseButtonWidth = 30f;
        private const float Margin = 25f;

        private const float DataPanelWidth = MasterPanelWidth - PlayerPanelWidth - Margin * 2;
        private const float DataPanelHeight = MasterPanelHeight - StatPanelHeight - Margin * 3;

        private UIPanel _statPanel;
        private UIPanel _playerPanel;
        private UIPanel _dataPanel;
        private UIText _awaitingText;
        private Dictionary<int, Dictionary<string, UIText>> _data;
        public static List<string> VisibleStats = new(Contribution.StatNames);

        public override void OnInitialize()
        {
            UIDragablePanel masterPanel = new() {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
            masterPanel.Width.Set(MasterPanelWidth, 0);
            masterPanel.Height.Set(MasterPanelHeight, 0);
            masterPanel.SetPadding(0f);
            Append(masterPanel);

            UIElement titleRect = new();
            titleRect.Width.Set(MasterPanelWidth, 0);
            titleRect.Height.Set(Margin * 2, 0);
            titleRect.SetPadding(0f);
            masterPanel.Append(titleRect);

            UIText title = new("Leaderboard") {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
            titleRect.Append(title);

            UIPanel closeButton = new() {
                VAlign = 0.5f
            };
            closeButton.Width.Set(CloseButtonWidth, 0);
            closeButton.Height.Set(CloseButtonWidth, 0);
            closeButton.Left.Set(-Margin - CloseButtonWidth / 2, 1);
            closeButton.OnLeftClick += OnCloseButtonClick;
            titleRect.Append(closeButton);

            UIText closeText = new("X") {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
            closeButton.Append(closeText);

            _playerPanel = new();
            _playerPanel.Width.Set(PlayerPanelWidth, 0);
            _playerPanel.Height.Set(DataPanelHeight, 0);
            _playerPanel.Top.Set(Margin * 2 + StatPanelHeight, 0);
            _playerPanel.Left.Set(Margin, 0);
            masterPanel.Append(_playerPanel);

            _statPanel = new();
            _statPanel.Width.Set(DataPanelWidth, 0);
            _statPanel.Height.Set(StatPanelHeight, 0);
            _statPanel.Top.Set(Margin * 2, 0);
            _statPanel.Left.Set(Margin + PlayerPanelWidth, 0);
            masterPanel.Append(_statPanel);

            _dataPanel = new();
            _dataPanel.Width.Set(DataPanelWidth, 0);
            _dataPanel.Height.Set(DataPanelHeight, 0);
            _dataPanel.Top.Set(Margin * 2 + StatPanelHeight, 0);
            _dataPanel.Left.Set(Margin + PlayerPanelWidth, 0);
            masterPanel.Append(_dataPanel);

            _awaitingText = new("Awaiting boss fight...") {
                HAlign = 0.5f,
                VAlign = 0.5f
            };

            _data = new();
        }

        private void OnCloseButtonClick(UIMouseEvent evt, UIElement listeningElement)
            => ModContent.GetInstance<LeaderboardSystem>().HideMyUI();

        public void AddStatHeadings()
        {
            foreach (string statName in VisibleStats) {
                UIText statHeading = new(statName) {
                    HAlign = 1f / VisibleStats.Count * (VisibleStats.IndexOf(statName) + 0.5f),
                    VAlign = 0.5f
                };
                _statPanel.Append(statHeading);
            }
        }

        public void AddPlayer(int whoAmI, Contribution contribution)
        {
            _data.Add(whoAmI, new());

            UIText playerName = new(Main.player[whoAmI].name) {
                HAlign = 0.5f,
                VAlign = (float)_data.Count / (Utilities.GetActivePlayers(Main.player).Count + 1)
            };
            _playerPanel.Append(playerName);

            foreach (string statName in Contribution.StatNames)
                AddCell(whoAmI, statName, contribution.GetStat(statName).ToString());
        }

        private void AddCell(int whoAmI, string statName, string value)
        {
            UIText statText = new(value) {
                HAlign = 1f / VisibleStats.Count * (VisibleStats.IndexOf(statName) + 0.5f),
                VAlign = (float)_data.Count / (Utilities.GetActivePlayers(Main.player).Count + 1)
            };
            _dataPanel.Append(statText);
            _data[whoAmI].Add(statName, statText);
        }

        public void UpdateCell(int whoAmI, string statName, object value)
            => _data[whoAmI][statName].SetText(value.ToString());

        public void RemoveAllData(bool showAwaitingText = false)
        {
            _playerPanel.RemoveAllChildren();
            _statPanel.RemoveAllChildren();

            foreach (var player in _data)
                foreach (var stat in player.Value)
                    _dataPanel.RemoveChild(stat.Value);
            _data.Clear();

            _awaitingText.Remove();
            if (showAwaitingText)
                _dataPanel.Append(_awaitingText);
        }
    }
}
