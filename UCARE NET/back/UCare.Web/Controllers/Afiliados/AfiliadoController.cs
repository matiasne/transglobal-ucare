using GQ.Security.Internal;
using Microsoft.AspNetCore.Mvc;
using UCare.Application.Users;
using UCare.Domain.Users;
using UCare.Infrastructure.Firebase;
using UCare.Shared.Domain.ValueObjects;
using UCare.Shared.Infrastructure;
using UCare.Web.Controllers.Afiliados.Dto;

namespace UCare.Web.Controllers.AlertasEstado
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [SecurityDescription("Afiliado Alertas", new object[] { Roles.Verificador })]
    public class AfiliadoController : Controller
    {
        private readonly AfiliadoVerificadorApp app;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public AfiliadoController(AfiliadoVerificadorApp app)
        {
            this.app = app;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpPost($"api/v1/[controller]")]
        public virtual async Task<IPaging> Find([FromBody] Paging<UsuarioAfiliado, AfiliadoDto> paging)
        {
            return await app.Get(paging);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpGet($"api/v1/[controller]/{{id}}")]
        public virtual async Task<AfiliadoDto> GetId(string id)
        {
            return new AfiliadoDto().SetEntity(await app.GetById(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SecurityDescription("Guardar", new object[] { Roles.Propietario, Roles.Gerente, Roles.Administrador, Roles.Verificador })]
        [HttpPut($"api/v1/[controller]")]
        public virtual async Task<AfiliadoDto> Save([FromBody] AfiliadoDto model)
        {
            return model.SetEntity(await app.Save(model.GetEntity()));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpPut($"api/v1/[controller]/[action]")]
        public virtual async Task<bool> ChangeState([FromBody] CambiarEstadoDto model)
        {
            return (await app.CambarEstado(model.GetEntity())) != null;
        }
    }
}
