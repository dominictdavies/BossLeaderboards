using Terraria.ModLoader;

namespace BossLeaderboards.Source.Common.Player
{
    internal class KeybindSystem : ModSystem
    {
        public static ModKeybind ShowLeaderboardKeybind { get; private set; }

        public override void Load()
            => ShowLeaderboardKeybind = KeybindLoader.RegisterKeybind(Mod, "ShowLeaderboard", "L");

        public override void Unload()
            => ShowLeaderboardKeybind = null;
    }
}
