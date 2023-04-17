using GQ.Data.Dto;
using UCare.Domain.Users;
using UCare.Web.Controllers.Authentication.Dto;

namespace UCare.Web.Controllers.Mobile.Dto
{
    public class DatosPersonalesDto : Dto<UsuarioAfiliado, DatosPersonalesDto>
    {
        public virtual string? Password { get; set; }
        public virtual string UsuarioNombre { get; set; }
        public virtual DateTime FechaNacimiento { get; set; }
        public virtual string? Email { get; set; }
        public virtual string Celular { get; set; }
        public virtual string CodigoPais { get; set; }
        public virtual string Sexo { get; set; }
        public virtual string Nosocomio { get; set; }
        public virtual DireccionDto Direccion { get; set; }
        public virtual GeoPositionDto Position { get; set; } = new GeoPositionDto();

        public override DatosPersonalesDto SetEntity(UsuarioAfiliado value)
        {
            var dto = base.SetEntity(value);
            dto.Password = null;
            return dto;
        }
    }

    public class TokenDto : Dto<UsuarioAfiliado, TokenDto>
    {
        public virtual string? Token { get; set; }
    }

}
