namespace MortyGame.Morties;

public class ClassicMorty(int boxCount) : IMorty
{
    public int ChooseRemainingBox(int gunBox, int rickBox)
    {
        if (gunBox == rickBox)
        {
            // If Rick has chosen the box with the gun, Morty can choose any other box
            Random rand = new Random();
            int choice;
            do
            {
                choice = rand.Next(boxCount);
            } while (choice == gunBox);
            return choice;
        }
        else
        {
            // If Rick has not chosen the box with the gun, Morty must choose the box with the gun
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
