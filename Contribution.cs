namespace Leaderboards
{
    public class Contribution
    {
        public int totalDamageTo;
        public int totalLifeLostFrom;

        public Contribution(int totalDamageTo = 0, int totalLifeLostFrom = 0)
        {
            this.totalDamageTo = totalDamageTo;
            this.totalLifeLostFrom = totalLifeLostFrom;
        }
    }
}
