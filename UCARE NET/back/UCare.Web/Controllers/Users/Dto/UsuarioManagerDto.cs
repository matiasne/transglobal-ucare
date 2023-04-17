using GQ.Data.Dto;
using UCare.Domain.Users;
using UCare.Shared.Domain;
using UCare.Shared.Domain.ValueObjects;
using UCare.Web.Controllers.Mapa.Dto;

namespace UCare.Web.Controllers.Users.Dto
{
    public class UsuarioManagerDto : Dto<UsuarioManager, UsuarioManagerDto>
    {
        public virtual string? Id { get; set; }
        public virtual string? UsuarioNombre { get; set; }
        public virtual string? Email { get; set; }
        public virtual string? Password { get; set; }
        public virtual string? Salt { get; set; }
        public virtual string Rol { get; set; } = string.Empty;

        public virtual List<string>? CodigoPostal { get; set; }
        public virtual string? UsuarioId { get; set; } // Usuario a quien pertenece
        public virtual MapaConfigDto Mapa { get; set; } = new MapaConfigDto();

        public virtual string Estado { get; set; } = Estados.Desactivo;
        public virtual Modificacion Creado { get; set; } = new Modificacion();
        public virtual Modificacion Modificacion { get; set; } = new Modificacion();

        public override UsuarioManagerDto SetEntity(UsuarioManager value)
        {
            var dto = base.SetEntity(value);
            dto.CodigoPostal = value.CodigoPostal;
            dto.Password = "";
            dto.Salt = "";
            return dto;
        }

        public override UsuarioManager GetEntity()
        {
            var entity = base.GetEntity();
            entity.CodigoPostal = CodigoPostal;
            return entity;
        }


    }
}
