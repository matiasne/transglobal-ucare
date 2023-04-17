using System;

namespace UCare.Shared.Domain.ValueObjects
{
    public static class Generos
    {
        public const string Masculino = "M";
        public const string Femenino = "F";
        public const string Otro = "O";

        public static string[] All { get; private set; } = new string[] { Masculino, Femenino, Otro };
    }
}
