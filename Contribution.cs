namespace Leaderboards
{
    public class Contribution
    {
        public int damage;
        public int kills;
        public int lifeLost;
        public int hitsTaken;
        public int deaths;

        public Contribution(int damage = 0, int kills = 0, int lifeLost = 0, int hitsTaken = 0, int deaths = 0)
        {
            this.damage = damage;
            this.kills = kills;
            this.lifeLost = lifeLost;
            this.hitsTaken = hitsTaken;
            this.deaths = deaths;
        }
    }
}
