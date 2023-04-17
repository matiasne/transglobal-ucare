using GQ.Data.Dto;
using UCare.Domain.Users;

namespace UCare.Web.Controllers.Users.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class UsuarioNameDto : Dto<UsuarioManager, UsuarioNameDto>
    {
        public virtual string? Id { get; set; }
        public virtual string? UsuarioNombre { get; set; }
    }
}
