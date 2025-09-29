namespace MortyGame.Utils;

public class ArgumentParser
{
    public int BoxCount { get; set; }
    public string MortyClassName { get; set; }

    public ArgumentParser(string[] args)
    {
        if (args.Length < 2)
        {
            throw new ArgumentException("Two arguments required: <boxCount> <MortyClassName>.");
        }
        if (!int.TryParse(args[0], out int boxCount) || boxCount < 3)
        {
            throw new ArgumentException("Box count must be an integer >= 3.");
        }
        BoxCount = boxCount;
        MortyClassName = args[1];
    }
}
