namespace UCare.Application.Users.Exceptions
{
    [Serializable]
    public class NumeroTelefonoException : Exception
    {
        public NumeroTelefonoException() : base("Formato del numero de telefono incorrecto") { }

        protected NumeroTelefonoException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}
