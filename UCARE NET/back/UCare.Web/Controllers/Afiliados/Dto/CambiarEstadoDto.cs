using GQ.Data.Dto;
using UCare.Domain.Users;
using UCare.Shared.Domain.ValueObjects;

namespace UCare.Web.Controllers.Afiliados.Dto
{
    public class CambiarEstadoDto : Dto<UsuarioAfiliado, AfiliadoDto>
    {
        public virtual string? Id { get; set; }
        public virtual string Estado { get; set; } = Estados.Desactivo;
    }
}
