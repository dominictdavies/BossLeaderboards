namespace Leaderboards
{
    public class Contribution
    {
        public long damage;
        public int kills;
        public long lifeLost;
        public int hitsTaken;
        public int deaths;

        public static Contribution Default = new Contribution();

        public Contribution(long damage = 0, int kills = 0, long lifeLost = 0, int hitsTaken = 0, int deaths = 0)
        {
            this.damage = damage;
            this.kills = kills;
            this.lifeLost = lifeLost;
            this.hitsTaken = hitsTaken;
            this.deaths = deaths;
        }

        public void Reset()
        {
            this.damage = Default.damage;
            this.kills = Default.kills;
            this.lifeLost = Default.lifeLost;
            this.hitsTaken = Default.hitsTaken;
            this.deaths = Default.deaths;
        }

        public bool IsEmpty()
        {
            return this.damage == Default.damage &&
                   this.kills == Default.kills &&
                   this.lifeLost == Default.lifeLost &&
                   this.hitsTaken == Default.hitsTaken &&
                   this.deaths == Default.deaths;
        }
    }
}
