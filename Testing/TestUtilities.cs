using NUnit.Framework;
using ReviveMod.Source.Common.Commands;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace ReviveMod.Testing.Common.Commands
{
    [TestFixture]
    internal class TestModCommandUtils
    {
        private Player[] _players;

        [SetUp]
        public void SetUp()
        {
            _players = new Player[Main.maxPlayers];

            for (int i = 0; i < Main.maxPlayers; i++) {
                _players[i] = new Player() { active = false };
            }
        }

        private void SetPlayerNames(string[] allNames)
        {
            int i = 0;
            foreach (string name in allNames) {
                _players[i].active = true;
                _players[i].name = name;
                i++;
            }
        }

        [TestCase("Doomimic", new string[0])]
        [TestCase("Doomimic", new string[] { "John" })]
        [TestCase("Doomimic", new string[] { "John", "Steven", "Emily", "doomimic", "DOOMIMIC" })]
        public void TryGetPlayer_ExcludedName_ReturnFalse(string excludedName, string[] allNames)
        {
            SetPlayerNames(allNames);

            Assert.IsFalse(ModCommandUtils.TryGetPlayer(excludedName, _players, out Player player));

            Assert.IsNull(player);
        }
    }
}
