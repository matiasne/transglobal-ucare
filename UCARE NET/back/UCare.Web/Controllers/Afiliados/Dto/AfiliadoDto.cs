using GQ.Data.Dto;
using UCare.Domain.Users;
using UCare.Shared.Domain;
using UCare.Shared.Domain.ValueObjects;
using UCare.Web.Controllers.Authentication.Dto;

namespace UCare.Web.Controllers.Afiliados.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class AfiliadoDto :Dto<UsuarioAfiliado, AfiliadoDto>
    {
        public virtual string? Id { get; set; }
        public virtual string? UsuarioNombre { get; set; }
        public virtual string? Email { get; set; }
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
        public virtual VerificacionDto? VerificaEmail { get; set; } = new VerificacionDto();
        public virtual VerificacionDto? VerificaTelefono { get; set; } = new VerificacionDto();
        public virtual string Estado { get; set; } = Estados.Desactivo;
        public virtual ModificacionDto? Creado { get; set; } = new ModificacionDto();
        public virtual ModificacionDto? Modificacion { get; set; } = new ModificacionDto();
    }


    public class ModificacionDto : Dto<Modificacion, ModificacionDto>
    {
        public string? UsuarioId { get; set; }
        public DateTime Modificado { get; set; } = DateTime.UtcNow;
    }
}
