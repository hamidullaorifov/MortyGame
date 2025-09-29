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
        var gunPlacementRng = new Security.FairRandomGenerator(boxCount);
        Console.WriteLine($"Morty: HMAC1: {gunPlacementRng.GetHmac()}");

        Console.Write($"Rick: Enter your number [0,{boxCount}): ");
        if (!int.TryParse(Console.ReadLine(), out int rickContribution1))
        {
            Console.WriteLine("Morty: Invalid input. Please enter a valid number.");
            return;
        }

        gunPlacementRng.RickValue = rickContribution1;
        int gunBox = gunPlacementRng.Finalize(rickContribution1);

        // Rick guesses a box
        Console.Write($"Rick: Choose a box [0,{boxCount}): ");
        if (!int.TryParse(Console.ReadLine(), out int rickInitialGuess) || rickInitialGuess < 0 || rickInitialGuess >= boxCount)
        {
            Console.WriteLine("Morty: Invalid input. Please enter a valid number.");
            return;
        }

        // Second round of fair randomness (to decide which box stays in play)
        var eliminationRng = new Security.FairRandomGenerator(boxCount - 1);
        Console.WriteLine($"Morty: HMAC2: {eliminationRng.GetHmac()}");

        Console.Write($"Rick: Enter your number [0,{boxCount - 1}): ");
        if (!int.TryParse(Console.ReadLine(), out int rickContribution2))
        {
            Console.WriteLine("Morty: Invalid input. Please enter a valid number.");
            return;
        }

        eliminationRng.RickValue = rickContribution2;
        eliminationRng.Finalize(rickContribution2);

        // Morty chooses which other box remains
        int offeredBox = morty.ChooseRemainingBox(gunBox, rickInitialGuess);
        Console.WriteLine($"Morty: You can keep box {rickInitialGuess} or switch to box {offeredBox}");

        // Rick decides whether to switch
        Console.Write("Rick: Switch? (y/n): ");
        bool switchChoice = Console.ReadLine()!.Trim().ToLower() == "y";
        int finalBoxChoice = switchChoice ? offeredBox : rickInitialGuess;

        // Reveal results
        gunPlacementRng.Reveal();
        Console.WriteLine($"Morty: Gun is in box {gunBox}");

        eliminationRng.Reveal();


        bool win = finalBoxChoice == gunBox;
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
