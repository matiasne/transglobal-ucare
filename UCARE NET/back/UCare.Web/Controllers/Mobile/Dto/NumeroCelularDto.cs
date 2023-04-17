using GQ.Data.Dto;
using UCare.Domain.Users;

namespace UCare.Web.Controllers.Mobile.Dto
{
    public class NumeroIdentidadDto : Dto<UsuarioAfiliado, NumeroIdentidadDto>
    {
        public virtual string? Password { get; set; }
        public virtual string NumeroIdentidad { get; set; }

        public override NumeroIdentidadDto SetEntity(UsuarioAfiliado value)
        {
            var dto = base.SetEntity(value);
            dto.Password = null;
            return dto;
        }
    }
}
