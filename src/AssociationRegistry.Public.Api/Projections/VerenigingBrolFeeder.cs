namespace AssociationRegistry.Public.Api.Projections;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

public interface IVerenigingBrolFeeder
{
    string KorteNaam { get; }
    string Hoofdlocatie { get; }
    ImmutableArray<string> Locaties { get; }
    string[] Hoofdactiviteiten { get; }
    string Doelgroep { get; }
    ImmutableArray<string> Activiteiten { get; }
}

public class VerenigingBrolFeeder : IVerenigingBrolFeeder
{
    private readonly List<string> _randomTexts = new()
    {
        "harbor     ",
        "veil       ",
        "control    ",
        "subway     ",
        "faint      ",
        "joy        ",
        "watch      ",
        "abbey      ",
        "far        ",
        "sink       ",
        "budget     ",
        "salmon     ",
        "facility   ",
        "parameter  ",
        "stumble    ",
        "evening    ",
        "valley     ",
        "branch     ",
        "network    ",
        "snail      ",
        "literacy   ",
        "limit      ",
        "gain       ",
        "need       ",
        "thirsty    ",
        "wine       ",
        "medieval   ",
        "poem       ",
        "diamond    ",
        "deserve    ",
        "pottery    ",
        "ankle      ",
        "soup       ",
        "trade      ",
        "relate     ",
        "relief     ",
        "aisle      ",
        "warm       ",
        "clock      ",
        "premature  ",
        "polite     ",
        "diplomat   ",
        "soar       ",
    };

    private readonly Random _random = new();

    private string ComposeText(int maxNumberOfWords = 7)
    {
        var numberOfWords = _random.Next(1, maxNumberOfWords);
        var words = new string[numberOfWords];

        for (var i = 0; i < numberOfWords; i++)
        {
            words[i] = _randomTexts[_random.Next(_randomTexts.Count - 1)].TrimEnd();
        }

        return string.Join(' ', words);
    }

    private IEnumerable<string> ComposeArray(int maxNumberOfElements, Func<string> composeText)
    {
        var numberOfElements = _random.Next(1, maxNumberOfElements);

        for (var i = 0; i < numberOfElements; i++)
        {
            yield return composeText();
        }
    }

    private string GetHoofdactiviteit()
    {
        var hoofdactiviteiten = BrolFeederHoofdactiviteit.All();
        var index = _random.Next(hoofdactiviteiten.Count);

        return hoofdactiviteiten[index].ToString();
    }

    public string KorteNaam
        => ComposeText(1);

    public string Hoofdlocatie
        => ComposeText(3);

    public ImmutableArray<string> Locaties
        => ComposeArray(3, () => ComposeText(1))
            .ToImmutableList()
            .Add(Hoofdlocatie)
            .ToImmutableArray();

    public string[] Hoofdactiviteiten
        => ComposeArray(2, GetHoofdactiviteit).ToArray();

    public string Doelgroep
        => ComposeText(1);

    public ImmutableArray<string> Activiteiten
        => ComposeArray(3, () => ComposeText(1)).ToImmutableArray();
}