[System.Serializable]
public class Player
{
    public int yards, energy;
    public int ID, power;
    public bool myTurn, playedCard;


    public Player(int yards, int energy, int ID, int power, bool playedCard)
    {
        this.yards = yards;
        this.energy = energy;
        this.ID = ID;
        this.power = power;
        this.playedCard = playedCard;
    }
}
