using System.Security.Cryptography;
using System.Text;
namespace MortyGame.Security;

public class FairRandomGenerator
{
    private readonly int _range;
    private readonly byte[] _secretKey;
    private readonly int _mortyValue;

    public FairRandomGenerator(int range)
    {
        _range = range;
        _secretKey = RandomNumberGenerator.GetBytes(32);
        _mortyValue = RandomNumberGenerator.GetInt32(_range);
    }

    public string GetHmac()
    {
        using var hmac = new HMACSHA256(_secretKey);
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(_mortyValue.ToString()));
        return Convert.ToHexString(hash);
    }

    public int Finalize(int rickValue)
    {
        return (_mortyValue + rickValue) % _range;
    }
    public void Reveal()
    {
        Console.WriteLine($"Morty: My secret value was {_mortyValue}");
        Console.WriteLine($"Morty: Key = {Convert.ToHexString(_secretKey)}");
        Console.WriteLine($"Morty: Fair result = (m + r) % {_range} = {_mortyValue}");
    }
}
