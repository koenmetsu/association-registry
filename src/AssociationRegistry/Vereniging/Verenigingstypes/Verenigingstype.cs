﻿namespace AssociationRegistry.Vereniging;

public class Verenigingstype : IVerenigingstype
{
    public static readonly Verenigingstype FeitelijkeVereniging = new(code: "FV", naam: "Feitelijke vereniging");
    public static readonly Verenigingstype VZER = new(code: "VZER", naam: "Vereniging zonder eigen rechtspersoonlijkheid");
    public static readonly Verenigingstype VZW = new(code: "VZW", naam: "Vereniging zonder winstoogmerk");
    public static readonly Verenigingstype IVZW = new(code: "IVZW", naam: "Internationale vereniging zonder winstoogmerk");
    public static readonly Verenigingstype PrivateStichting = new(code: "PS", naam: "Private stichting");
    public static readonly Verenigingstype StichtingVanOpenbaarNut = new(code: "SVON", naam: "Stichting van openbaar nut");

    public static Verenigingstype[] All =>
    [
        FeitelijkeVereniging,
        VZER,
        VZW,
        IVZW,
        PrivateStichting,
        StichtingVanOpenbaarNut,
    ];

    public Verenigingstype(string code, string naam)
    {
        Code = code;
        Naam = naam;
    }

    public string Code { get; init; }
    public string Naam { get; init; }

    public static Verenigingstype Parse(string code)
        => All.Single(t => t.Code == code);

    public static bool IsGeenKboVereniging(string code)
        => code == FeitelijkeVereniging.Code || code == VZER.Code;

    public static bool IsKboVereniging(string code)
        => !IsGeenKboVereniging(code);

    public static bool TypeIsVerenigingZonderEigenRechtspersoonlijkheid(Verenigingstype type)
        => type == FeitelijkeVereniging || type == VZER;
}
