namespace UCare.Application.Users.Exceptions
{
    [Serializable]
    public class PasswordIncorrectoException : Exception
    {
        public PasswordIncorrectoException() : base("Clave incorrecta") { }

        protected PasswordIncorrectoException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}
