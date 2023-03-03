[System.Serializable]
public class Player
{
    public int yards, energy;
    public int ID, power, linePower, athPower, dmPower;
    public bool isOffense, playedTactic;
    


    public Player(int yards, int energy, int ID, int power, int linePower, int athPower, int dmPower, bool playedTactic, bool isOffense)
    {
        this.yards = yards;
        this.energy = energy;
        this.ID = ID;
        this.power = power;
        this.linePower = linePower;
        this.athPower = athPower;
        this.dmPower = dmPower;
        this.playedTactic = playedTactic;
        this.isOffense = isOffense;
    }
}
