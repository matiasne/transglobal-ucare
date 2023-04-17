using GQ.Data.Dto;
using UCare.Domain.Alertas;
using UCare.Domain.Users;

namespace UCare.Web.Controllers.Authentication.Dto
{
    public class UsuarioAfiliadoDto : Dto<UsuarioAfiliado, UsuarioAfiliadoDto>
    {
        public virtual string Id { get; set; }
        public virtual string Signature { get; set; }
        public virtual string? UsuarioNombre { get; set; }
        public virtual string? Email { get; set; }
        public virtual string? Password { get; set; }
        public virtual string NumeroIdentidad { get; set; }
        public virtual DateTime FechaNacimiento { get; set; }
        public virtual string Sexo { get; set; }
        public virtual DireccionDto Direccion { get; set; }
        public virtual GeoPositionDto? Position { get; set; } = new GeoPositionDto();
        public virtual string Celular { get; set; }
        public virtual string CodigoPais { get; set; }
        public virtual string? Nosocomio { get; set; }
        public virtual List<string>? Patologias { get; set; }
        public virtual List<string>? Medicacion { get; set; }
        public virtual List<string>? Alergias { get; set; }
        public virtual List<ContactoDto>? Contactos { get; set; }
        public virtual AfiliacionDto? Afiliacion { get; set; }

        public override UsuarioAfiliado GetEntity()
        {
            var dto = base.GetEntity();
            dto.Patologias = Patologias;
            dto.Alergias = Alergias;
            dto.Medicacion = Medicacion;
            return dto;
        }

        public override UsuarioAfiliadoDto SetEntity(UsuarioAfiliado value)
        {
            var dto = base.SetEntity(value);
            dto.Patologias = value.Patologias;
            dto.Alergias = value.Alergias;
            dto.Medicacion = value.Medicacion;
            return dto;
        }
    }

    public class VerificacionDto : Dto<Verificacion, VerificacionDto>
    {
        public bool Verificado { get; set; } = false;
        public string CodigoVerificacion { get; set; } = string.Empty;
    }

    public class GeoPositionDto : Dto<GeoPosition, GeoPositionDto>
    {
        public virtual string? Id { get; set; } = string.Empty;
        public virtual double? Lat { get; set; } = double.MaxValue;
        public virtual double? Lon { get; set; } = double.MaxValue;
    }

    public class DireccionDto : Dto<Direccion, DireccionDto>
    {
        public virtual string? Calle { get; set; }
        public virtual string? Nro { get; set; }
        public virtual string? Piso { get; set; }
        public virtual string? Barrio { get; set; }
        public virtual string? Ciudad { get; set; }
        public virtual string? Departamento { get; set; }
        public virtual string? CodigoPostal { get; set; }
    }

    public class ContactoDto : Dto<Contacto, ContactoDto>
    {
        public virtual string Nombre { get; set; }
        public virtual string Telefono { get; set; }
    }

    public class AfiliacionDto : Dto<Afiliacion, AfiliacionDto>
    {
        public virtual string Empresa { get; set; }
        public virtual string Servicio { get; set; }
    }
}
