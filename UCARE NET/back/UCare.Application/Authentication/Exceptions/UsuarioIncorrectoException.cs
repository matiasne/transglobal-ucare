namespace UCare.Application.Authentication.Exceptions
{
    [Serializable]
    public class UsuarioIncorrectoException : Exception
    {
        public UsuarioIncorrectoException() : base("Email o clave incorrecta") { }

        protected UsuarioIncorrectoException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }

    [Serializable]
    public class UsuarioInactivoException : Exception
    {
        public UsuarioInactivoException() : base("El usuario no se encuentra activo") { }

        protected UsuarioInactivoException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}

