namespace MortyGame.Game;

public class Statistics
{
    public int SwitchWins { get; set; }
    public int StayWins { get; set; }
    public int SwitchCount { get; set; }
    public int StayCount { get; set; }
    public int TotalRounds { get; set; }

    public void Record(bool switched, bool won)
    {
        TotalRounds++;
        if (switched)
        {
            SwitchCount++;
            if (won) SwitchWins++;
        }
        else
        {
            StayCount++;
            if (won) StayWins++;
        }
    }
}
