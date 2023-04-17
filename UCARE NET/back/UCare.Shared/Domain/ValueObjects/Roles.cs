namespace UCare.Shared.Domain.ValueObjects
{
    public static class Roles
    {
        public const string Propietario = "P"; // Maximo de usuario activos que va a tener solo crean los gerentes

        public const string Gerente = "G"; // Define los códigos postales a los administradores agrega administradores

        public const string Administrador = "A";

        public const string Monitor = "M";
        public const string Verificador = "V";

        public const string Afiliado = "U";
    }
}
