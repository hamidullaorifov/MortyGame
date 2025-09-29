using MortyGame.Game;
using MortyGame.Morties;
using MortyGame.Utils;

try
{
    var parser = new ArgumentParser(args);
    int boxCount = parser.BoxCount;
    string mortyClassName = parser.MortyClassName;

    var morty = MortyFactory.CreateMorty(mortyClassName, boxCount);
    var game = new Game(boxCount, morty);
    game.Run();
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine("Example usage:");
    Console.WriteLine("  dotnet run 3 ClassicMorty");
    Console.WriteLine("  dotnet run 3 LazyMorty");
}