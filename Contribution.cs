namespace Leaderboards
{
    public class Contribution
    {
        public int bossWhoAmI;
        public int totalDamage;
        public int totalLifeLost;

        public Contribution(int bossWhoAmI, int totalDamage = 0, int totalLifeLost = 0)
        {
            this.bossWhoAmI = bossWhoAmI;
            this.totalDamage = totalDamage;
            this.totalLifeLost = totalLifeLost;
        }
    }
}
