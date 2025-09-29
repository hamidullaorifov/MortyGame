using ConsoleTables;
using MortyGame.Morties;

namespace MortyGame.Game;

public class Game(int boxCount, IMorty morty)
{
    private readonly Statistics _stats = new Statistics();
    
    public void Run()
    {
        while (true)
        {
            PlayRound();
            Console.WriteLine("Play again? (y/n): ");
            string input = Console.ReadLine()!.Trim().ToLower();
            if (input == "n") break;
        }
        ShowStatistics();
    }
    private void PlayRound()
    {
        Console.WriteLine($"Morty: Hiding gun in one of {boxCount} boxes...");
        var rng1 = new Security.FairRandomGenerator(boxCount);
        Console.WriteLine($"Morty: HMAC1: {rng1.GetHmac()}");
        Console.Write($"Rick: Enter your number [0,{boxCount}): ");
        if (!int.TryParse(Console.ReadLine(), out int r))
        {
            Console.WriteLine("Morty: Invalid input. Please enter a valid number.");
            return;
        }
        int gunBox = rng1.Finalize(r);

        Console.Write($"Rick: Choose a box [0,{boxCount}): ");
        if (!int.TryParse(Console.ReadLine(), out int rickBox) || rickBox < 0 || rickBox >= boxCount)
        {
            Console.WriteLine("Morty: Invalid input. Please enter a valid number.");
            return;
        }
        var rng2 = new Security.FairRandomGenerator(boxCount - 1);
        Console.WriteLine($"Morty: HMAC2: {rng2.GetHmac()}");

        Console.Write($"Rick: Enter your number [0,{boxCount - 1}): ");
        Console.ReadLine();
        int otherBox = morty.ChooseRemainingBox(gunBox, rickBox);
        Console.WriteLine($"Morty: You can keep box {rickBox} or switch to box {otherBox}");

        Console.Write("Rick: Switch? (y/n): ");
        bool switchChoice = Console.ReadLine()!.Trim().ToLower() == "y";
        int finalBox = switchChoice ? otherBox : rickBox;

        rng1.Reveal();
        Console.WriteLine($"Morty: Gun is in box {gunBox}");

        rng2.Reveal();
        
        bool win = finalBox == gunBox;
        Console.WriteLine(win ? "Rick wins!" : "Morty wins!");

        _stats.Record(switchChoice, win);
    }
    private void ShowStatistics()
    {
        var table = new ConsoleTable("Game results", "Rick switched", "Rick stayed");
        table.AddRow("Rounds", _stats.SwitchCount, _stats.StayCount)
             .AddRow("Wins", _stats.SwitchWins, _stats.StayWins)
             .AddRow("P (estimate)",
                    _stats.SwitchCount == 0 ? 0 : (double)_stats.SwitchWins / _stats.SwitchCount,
                    _stats.StayCount == 0 ? 0 : (double)_stats.StayWins / _stats.StayCount)
             .AddRow("P (exact)",
                    morty.GetExactSwitchProbability(boxCount),
                    morty.GetExactStayProbability(boxCount));
        table.Write();
    }
}
