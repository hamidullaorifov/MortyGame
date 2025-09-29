namespace MortyGame.Morties;

public class MortyFactory
{
    public static IMorty CreateMorty(string className, int boxCount)
    {
        return className switch
        {
            "ClassicMorty" => new ClassicMorty(boxCount),
            "LazyMorty" => new LazyMorty(boxCount),
            _ => throw new ArgumentException($"Unknown Morty class name: {className}")
        };
    }
}
