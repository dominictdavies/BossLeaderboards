using System.Collections.Generic;
using System.Linq;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;

namespace Leaderboards.UI
{
    internal class UILeaderboard : UIState
    {
        private const float MasterPanelWidth = 600f;
        private const float MasterPanelHeight = 300f;
        private const float DataPanelWidth = MasterPanelWidth - 20f;
        private const float DataPanelHeight = MasterPanelHeight - 80f;
        private UIPanel _dataPanel;
        private Dictionary<int, Dictionary<string, UIText>> _data;
        public static List<string> VisibleStats = Contribution.StatNames.ToList<string>();

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
            closeButton.OnClick += OnCloseButtonClick;
            masterPanel.Append(closeButton);

            UIText closeText = new UIText("X");
            closeText.HAlign = closeText.VAlign = 0.5f;
            closeButton.Append(closeText);

            _dataPanel = new UIPanel();
            _dataPanel.Width.Set(DataPanelWidth, 0);
            _dataPanel.Height.Set(DataPanelHeight, 0);
            _dataPanel.Top.Set(55, 0);
            _dataPanel.HAlign = 0.5f;
            masterPanel.Append(_dataPanel);

            _data = new Dictionary<int, Dictionary<string, UIText>>();
        }

        private void OnCloseButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            ModContent.GetInstance<LeaderboardSystem>().HideMyUI();
            SoundEngine.PlaySound(SoundID.MenuClose);
        }

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
            statText.HAlign = 1f / VisibleStats.Count * ((float)VisibleStats.IndexOf(statName) + 0.5f);
            _dataPanel.Append(statText);
            _data[whoAmI].Add(statName, statText);
        }

        public void UpdateCell(int whoAmI, string statName, object value)
            => _data[whoAmI][statName].SetText(value.ToString());

        public void RemoveData(bool showAwaitingText = false)
        {
            _dataPanel.RemoveAllChildren();
            _data.Clear();

            if (showAwaitingText)
            {
                UIText awaitingText = new UIText("Awaiting boss fight...");
                awaitingText.HAlign = awaitingText.VAlign = 0.5f;
                _dataPanel.Append(awaitingText);
            }
        }
    }
}
