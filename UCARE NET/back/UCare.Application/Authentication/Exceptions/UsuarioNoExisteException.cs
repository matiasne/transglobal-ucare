namespace UCare.Application.Authentication.Exceptions
{
    [Serializable]
    public class UsuarioNoExisteException : Exception
    {
        public UsuarioNoExisteException() : base("No se encontro el usuario") { }

        protected UsuarioNoExisteException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}
