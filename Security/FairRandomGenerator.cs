using System.Security.Cryptography;
using System.Text;
namespace MortyGame.Security;

public class FairRandomGenerator
{
    public int Range { get; set; }
    public int MortyValue { get; set; }
    public int RickValue { get; set; }
    public int FinalValue { get; set; }

    private readonly byte[] _secretKey;
    

    public FairRandomGenerator(int range)
    {
        Range = range;
        _secretKey = RandomNumberGenerator.GetBytes(32);
        MortyValue = RandomNumberGenerator.GetInt32(range);
    }

    public string GetHmac()
    {
        using var hmac = new HMACSHA256(_secretKey);
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(MortyValue.ToString()));
        return Convert.ToHexString(hash);
    }

    public int Finalize(int rickValue)
    {
        FinalValue = (MortyValue + rickValue) % Range;
        return FinalValue;
    }
    public void Reveal()
    {
        Console.WriteLine($"Morty: My secret value was {MortyValue}");
        Console.WriteLine($"Morty: Key = {Convert.ToHexString(_secretKey)}");
        Console.WriteLine($"Morty: Fair result = ({MortyValue} + {RickValue}) % {Range} = {FinalValue}");
    }
}
