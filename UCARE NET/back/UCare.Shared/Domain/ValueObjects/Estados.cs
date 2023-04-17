namespace UCare.Shared.Domain.ValueObjects
{
    public static class Estados
    {
        public const string Activo = "A"; // Activo
        public const string Revision = "R"; // Usuario Falta verificar los datos
        public const string Desactivo = "D"; // Desactivo
        public const string Borrado = "B"; // Borrado
        public const string Bloqueado = "K"; //Usuario Bloqueado
        public const string SinVerificar = "S"; //Usuario Bloqueado

        public static string[] AllEstadosUsuario { get; private set; } = new string[] { Activo, Revision, Desactivo, Bloqueado, Borrado, SinVerificar };

        public const string NoLeido = "N";
        public const string Leido = "L";

        public const string SinAsignar = "S";
        public const string Emergencia = "E";
        public const string Urgencia = "U";
        public const string FalsaAlarma = "F";
    }
}
