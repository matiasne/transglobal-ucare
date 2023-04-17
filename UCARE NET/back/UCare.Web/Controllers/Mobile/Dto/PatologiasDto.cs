using GQ.Data.Abstractions.Dto;
using GQ.Data.Abstractions.Entity;
using GQ.Data.Dto;
using UCare.Domain.Users;

namespace UCare.Web.Controllers.Mobile.Dto
{
    public class PatologiasDto : Dto<UsuarioAfiliado, PatologiasDto>
    {
        public virtual string? Password { get; set; }
        public virtual string Nosocomio { get; set; }
        public virtual List<string> Patologias { get; set; }
        public virtual List<string> Medicacion { get; set; }
        public virtual List<string> Alergias { get; set; }

        public override PatologiasDto SetEntity(UsuarioAfiliado value)
        {
            var dto = base.SetEntity(value);
            dto.Nosocomio = value.Nosocomio;
            dto.Medicacion = value.Medicacion;
            dto.Patologias = value.Patologias;
            dto.Alergias = value.Alergias;
            dto.Password = null;
            return dto;
        }

        public override UsuarioAfiliado GetEntity()
        {
            var entity = base.GetEntity();
            entity.Nosocomio = Nosocomio;
            entity.Medicacion = Medicacion;
            entity.Patologias = Patologias;
            entity.Alergias = Alergias;
            return entity;
        }
    }
}
