using GQ.Data.Dto;
using UCare.Domain.Users;

namespace UCare.Web.Controllers.Mobile.Dto
{
    public class CambioContrasena : Dto<UsuarioAfiliado, CambioContrasena>
    {
        public virtual string? Password { get; set; }
        public virtual string NewPassword { get; set; }

        public override CambioContrasena SetEntity(UsuarioAfiliado value)
        {
            var dto = base.SetEntity(value);
            dto.Password = null;
            return dto;
        }

        public override UsuarioAfiliado GetEntity()
        {
            var entity = base.GetEntity();
            entity.Salt = NewPassword;
            return entity;
        }
    }
}
