namespace Leaderboards
{
    public class Contribution
    {
        public int totalDamage;
        public int totalLifeLost;

        public Contribution(int totalDamage = 0, int totalLifeLost = 0)
        {
            this.totalDamage = totalDamage;
            this.totalLifeLost = totalLifeLost;
        }
    }
}
