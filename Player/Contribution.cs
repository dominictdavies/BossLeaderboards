namespace Leaderboards
{
    internal class Contribution
    {
        public long damage;
        public int kills;
        public long lifeLost;
        public int hitsTaken;
        public int deaths;

        public Contribution(long damage = 0, int kills = 0, long lifeLost = 0, int hitsTaken = 0, int deaths = 0)
        {
            this.damage = damage;
            this.kills = kills;
            this.lifeLost = lifeLost;
            this.hitsTaken = hitsTaken;
            this.deaths = deaths;
        }
    }
}
