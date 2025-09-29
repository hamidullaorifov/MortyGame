namespace MortyGame.Morties;

public interface IMorty
{
    int ChooseRemainingBox(int gunBox, int rickBox);
    double GetExactSwitchProbability(int boxCount);
    double GetExactStayProbability(int boxCount);
}
