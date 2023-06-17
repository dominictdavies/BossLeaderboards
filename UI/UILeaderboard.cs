﻿using System.Collections.Generic;
using System.Linq;
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
        private const float _masterPanelWidth = 600f;
        private const float _masterPanelHeight = 300f;
        private const float _dataPanelWidth = _masterPanelWidth - 20f;
        private const float _dataPanelHeight = _masterPanelHeight - 80f;
        private UIPanel _dataPanel;
        private Dictionary<int, Dictionary<string, UIText>> _data = new();

        public override void OnInitialize()
        {
            UIDragablePanel masterPanel = new UIDragablePanel();
            masterPanel.Width.Set(_masterPanelWidth, 0);
            masterPanel.Height.Set(_masterPanelHeight, 0);
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
            _dataPanel.Width.Set(_dataPanelWidth, 0);
            _dataPanel.Height.Set(_dataPanelHeight, 0);
            _dataPanel.Top.Set(55, 0);
            _dataPanel.HAlign = 0.5f;
            masterPanel.Append(_dataPanel);
        }

        private void OnCloseButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            ModContent.GetInstance<UILeaderboardSystem>().HideMyUI();
            SoundEngine.PlaySound(SoundID.MenuClose);
        }

        public void AddPlayer(int whoAmI)
        {
            RemovePlayer(whoAmI);

            int row = _data.Count;
            Dictionary<string, UIText> playerData = new();
            _data.Add(whoAmI, playerData);

            Player player = Main.player[whoAmI];
            LeaderboardsPlayer leaderboardsPlayer = player.GetModPlayer<LeaderboardsPlayer>();
            Dictionary<string, long> contribution = leaderboardsPlayer.contribution;
            string[] statNames = contribution.Keys.ToArray();
            for (int column = 0; column < statNames.Length; column++)
            {
                string name = statNames[column];
                UIText stat = new UIText(contribution[name].ToString());
                stat.VAlign = 0.1f * row;
                stat.HAlign = 1f / statNames.Length * ((float)column + 0.5f);
                _dataPanel.Append(stat);
                playerData.Add(name, stat);
            }
        }

        public void RemovePlayer(int whoAmI)
        {
            if (!_data.ContainsKey(whoAmI))
                return;

            Dictionary<string, UIText> playerData = _data[whoAmI];
            foreach (UIText stat in playerData.Values)
                _dataPanel.RemoveChild(stat);

            _data.Remove(whoAmI);
        }

        public void UpdateCell(int whoAmI, string statName, long value)
        {
            _data[whoAmI][statName].SetText(value.ToString());
        }
    }
}
