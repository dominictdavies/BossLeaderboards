using System.Collections.Generic;
using System.Linq;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using BossLeaderboards.UI;
using BossLeaderboards.Source.Common.Player;

namespace BossLeaderboards.Source.Common.UI
{
    internal class UILeaderboard : UIState
    {
        private const float MasterPanelWidth = 600f;
        private const float MasterPanelHeight = 300f;
        private const float DataPanelWidth = MasterPanelWidth - 20f;
        private const float DataPanelHeight = MasterPanelHeight - 80f;
        private const float PlayerPanelWidth = 80f;
        private const float StatPanelHeight = 40f;
        private UIPanel _dataPanel;
        private UIPanel _playerPanel;
        private UIPanel _statPanel;
        private UIText _awaitingText;
        private Dictionary<int, Dictionary<string, UIText>> _data;
        public static List<string> VisibleStats = Contribution.StatNames.ToList();

        public override void OnInitialize()
        {
            UIDragablePanel masterPanel = new UIDragablePanel();
            masterPanel.Width.Set(MasterPanelWidth, 0);
            masterPanel.Height.Set(MasterPanelHeight, 0);
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
            closeButton.OnLeftClick += OnCloseButtonClick;
            masterPanel.Append(closeButton);

            UIText closeText = new UIText("X");
            closeText.HAlign = closeText.VAlign = 0.5f;
            closeButton.Append(closeText);

            _dataPanel = new UIPanel();
            _dataPanel.Width.Set(DataPanelWidth, 0);
            _dataPanel.Height.Set(DataPanelHeight, 0);
            _dataPanel.Top.Set(55f, 0);
            _dataPanel.Left.Set(0f, 0);
            masterPanel.Append(_dataPanel);

            _playerPanel = new UIPanel();
            _playerPanel.Width.Set(PlayerPanelWidth, 0);
            _playerPanel.Height.Set(DataPanelHeight, 0);
            _playerPanel.Top.Set(_dataPanel.Top.Pixels, 0);
            _playerPanel.Left.Set(_dataPanel.Left.Pixels - PlayerPanelWidth, 0);
            masterPanel.Append(_playerPanel);

            _statPanel = new UIPanel();
            _statPanel.Width.Set(DataPanelWidth, 0);
            _statPanel.Height.Set(StatPanelHeight, 0);
            _statPanel.Top.Set(_dataPanel.Top.Pixels - StatPanelHeight, 0);
            _statPanel.Left.Set(_dataPanel.Left.Pixels, 0);
            masterPanel.Append(_statPanel);

            _data = new Dictionary<int, Dictionary<string, UIText>>();

            _awaitingText = new UIText("Awaiting boss fight...");
            _awaitingText.HAlign = _awaitingText.VAlign = 0.5f;
        }

        private void OnCloseButtonClick(UIMouseEvent evt, UIElement listeningElement)
            => ModContent.GetInstance<LeaderboardSystem>().HideMyUI();

        public void AddPlayer(int whoAmI, Contribution contribution)
        {
            _data.Add(whoAmI, new Dictionary<string, UIText>());

            foreach (string statName in Contribution.StatNames)
                AddCell(whoAmI, statName, contribution.GetStat(statName).ToString());
        }

        private void AddCell(int whoAmI, string statName, string value)
        {
            UIText statText = new UIText(value);
            statText.VAlign = 0.1f * (_data.Count - 1);
            statText.HAlign = 1f / VisibleStats.Count * (VisibleStats.IndexOf(statName) + 0.5f);
            _dataPanel.Append(statText);
            _data[whoAmI].Add(statName, statText);
        }

        public void UpdateCell(int whoAmI, string statName, object value)
            => _data[whoAmI][statName].SetText(value.ToString());

        public void RemoveAllData(bool showAwaitingText = false)
        {
            foreach (KeyValuePair<int, Dictionary<string, UIText>> player in _data)
                foreach (KeyValuePair<string, UIText> stat in player.Value)
                    _dataPanel.RemoveChild(stat.Value);
            _data.Clear();

            _awaitingText.Remove();
            if (showAwaitingText)
                _dataPanel.Append(_awaitingText);
        }
    }
}
