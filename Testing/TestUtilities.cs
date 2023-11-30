using BossLeaderboards.Source;
using NUnit.Framework;
using System.Linq;
using Terraria;

namespace BossLeaderboards.Testing
{
    [TestFixture]
    internal class TestUtilities
    {
        private Player[] _players;

        [SetUp]
        public void SetUp()
        {
            _players = new Player[Main.maxPlayers];

            for (int i = 0; i < Main.maxPlayers; i++) {
                _players[i] = new() { active = false, whoAmI = i };
            }
        }

        [Test]
        public void GetActivePlayers_NoneActive_ReturnEmpty()
        {
            var actualActivePlayers = Utilities.GetActivePlayers(_players);

            Assert.That(actualActivePlayers, Is.Empty);
        }

        [TestCase(new int[] { 36 })]
        [TestCase(new int[] { 0, 1, 2, 3, 4 })]
        [TestCase(new int[] { 4, 9, 27, 36, 100 })]
        public void GetActivePlayers_SomeActive_ReturnActivePlayers(int[] activeWhoAmIs)
        {
            foreach (int activeWhoAmI in activeWhoAmIs) {
                _players[activeWhoAmI].active = true;
            }

            var actualActiveIndices = Utilities.GetActivePlayers(_players)
                                      .Where(player => player.active)
                                      .Select(player => player.whoAmI)
                                      .ToList();

            Assert.That(activeWhoAmIs, Is.EqualTo(actualActiveIndices));
        }

        [Test]
        public void GetActivePlayers_AllActive_ReturnAllPlayers()
        {
            foreach (Player player in _players) {
                player.active = true;
            }

            var actualActivePlayers = Utilities.GetActivePlayers(_players);

            Assert.That(_players, Is.EqualTo(actualActivePlayers));
        }
    }
}
