namespace MortyGame.Morties;

public class LazyMorty(int boxCount) : IMorty
{
    public int ChooseRemainingBox(int gunBox, int rickBox)
    {
        if (gunBox == rickBox)
        {
            return rickBox == 0 ? 1 : 0;
        }
        else
        {
            return gunBox;
        }
    }

    public double GetExactStayProbability(int boxCount)
    {
        return 1.0 / boxCount;
    }

    public double GetExactSwitchProbability(int boxCount)
    {
        return 1.0 - 1.0 / boxCount;
    }
}
