using GQ.Data.Dto;
using UCare.Domain.Users;
using UCare.Web.Controllers.Authentication.Dto;

namespace UCare.Web.Controllers.Mobile.Dto
{
    public class ContactosEmergenciaDto : Dto<UsuarioAfiliado, ContactosEmergenciaDto>
    {
        public virtual string? Password { get; set; }
        public virtual List<ContactoDto> Contactos { get; set; }

        public override ContactosEmergenciaDto SetEntity(UsuarioAfiliado value)
        {
            var dto = base.SetEntity(value);
            dto.Password = null;
            return dto;
        }
    }
}
