namespace UCare.Application.Authentication.Exceptions
{
    [Serializable]
    public class UsuarioValidarEmailException : Exception
    {
        public UsuarioValidarEmailException() : base("Email no validado") { }

        protected UsuarioValidarEmailException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}
