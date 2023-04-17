namespace UCare.Web.Controllers.Authentication.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class RecupearDto
    {
        public string Username { get; set; }
        public string Signature { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class VerificarCodigoRecuperarDto
    {
        public string Id { get; set; }
        public string Codigo { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CambiarPasswordCodigoRecuperarDto : VerificarCodigoRecuperarDto
    {
        public string Password { get; set; }
    }
}
