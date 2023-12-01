using BossLeaderboards.UI;
using BossLeaderboards.Source.Common.Player;
using System.Collections.Generic;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace BossLeaderboards.Source.Common.UI
{
    internal class UILeaderboard : UIState
    {
        private const float MasterPanelWidth = 600f;
        private const float MasterPanelHeight = 300f;
        private const float PlayerPanelWidth = 80f;
        private const float StatPanelHeight = 40f;
        private const float CloseButtonWidth = 30f;
        private const float Margin = 30f;

        private const float DataPanelWidth = MasterPanelWidth - PlayerPanelWidth - Margin * 2;
        private const float DataPanelHeight = MasterPanelHeight - StatPanelHeight - Margin * 3;

        private UIPanel _dataPanel;
        private UIPanel _playerPanel;
        private UIPanel _statPanel;
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
            Append(masterPanel);

            UIText title = new("Leaderboard") {
                HAlign = 0.5f
            };
            title.Top.Set(Margin, 0);
            masterPanel.Append(title);

            UIPanel closeButton = new();
            closeButton.Width.Set(CloseButtonWidth, 0);
            closeButton.Height.Set(CloseButtonWidth, 0);
            closeButton.Top.Set(Margin, 0);
            closeButton.Left.Set(-Margin - CloseButtonWidth, 1);
            closeButton.OnLeftClick += OnCloseButtonClick;
            masterPanel.Append(closeButton);

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

            _data = new();

            _awaitingText = new("Awaiting boss fight...") {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
        }

        private void OnCloseButtonClick(UIMouseEvent evt, UIElement listeningElement)
            => ModContent.GetInstance<LeaderboardSystem>().HideMyUI();

        public void AddPlayer(int whoAmI, Contribution contribution)
        {
            _data.Add(whoAmI, new());

            foreach (string statName in Contribution.StatNames)
                AddCell(whoAmI, statName, contribution.GetStat(statName).ToString());
        }

        private void AddCell(int whoAmI, string statName, string value)
        {
            UIText statText = new(value) {
                HAlign = 1f / VisibleStats.Count * (VisibleStats.IndexOf(statName) + 0.5f),
                VAlign = 0.1f * (_data.Count - 1)
            };
            _dataPanel.Append(statText);
            _data[whoAmI].Add(statName, statText);
        }

        public void UpdateCell(int whoAmI, string statName, object value)
            => _data[whoAmI][statName].SetText(value.ToString());

        public void RemoveAllData(bool showAwaitingText = false)
        {
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
