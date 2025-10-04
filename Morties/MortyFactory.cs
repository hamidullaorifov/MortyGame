using System.Reflection;

namespace MortyGame.Morties;

public class MortyFactory
{
    public static IMorty CreateMorty(string className, int boxCount)
    {
        var mortyType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => typeof(IMorty).IsAssignableFrom(t) &&
                                     !t.IsInterface &&
                                     !t.IsAbstract &&
                                     t.Name.Equals(className, StringComparison.OrdinalIgnoreCase));

        if (mortyType == null)
            throw new ArgumentException($"Unknown Morty class name: {className}");

        return (IMorty)Activator.CreateInstance(mortyType, boxCount)!;
    }
}
