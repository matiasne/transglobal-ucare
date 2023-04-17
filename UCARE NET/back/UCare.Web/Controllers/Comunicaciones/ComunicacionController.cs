using GQ.Security.Internal;
using Microsoft.AspNetCore.Mvc;
using UCare.Application.Comunicaciones;
using UCare.Domain.Comunicaciones;
using UCare.Domain.Users;
using UCare.Infrastructure.Firebase;
using UCare.Shared.Domain.ValueObjects;
using UCare.Shared.Infrastructure;
using UCare.Web.Controllers.Afiliados.Dto;
using UCare.Web.Controllers.Comunicaciones.Dto;

namespace UCare.Web.Controllers.Comunicaciones
{
    [ApiController]
    [SecurityDescription("Comunicacion", new object[] { Roles.Administrador })]
    public class ComunicacionController : Controller
    {
        private readonly ComunicacionApp app;
        public ComunicacionController(ComunicacionApp app)
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
        public virtual async Task<IPaging> Find([FromBody] Paging<Comunicacion, ComunicacionDto> paging)
        {
            return await app.Get(paging);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpPost($"api/v1/[controller]/[action]")]
        public virtual async Task<IPaging> FindAfiliados([FromBody] Paging<UsuarioAfiliado, AfiliadoDto> paging)
        {
            return await app.GetAfiliados(paging);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpGet($"api/v1/[controller]/{{id}}")]
        public virtual async Task<ComunicacionDto> GetId(string id)
        {
            return new ComunicacionDto().SetEntity(await app.GetById(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SecurityDescription("Guardar", new object[] { Roles.Administrador })]
        [HttpPut($"api/v1/[controller]")]
        public virtual async Task<ComunicacionDto> Save([FromBody] ComunicacionDto model)
        {
            return model.SetEntity(await app.Save(model.GetEntity()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SecurityDescription("Borrar", new object[] { Roles.Administrador })]
        [HttpDelete($"api/v1/[controller]/{{id}}")]
        public virtual async Task<bool> Delete(string id)
        {
            await app.Delete(id);
            return true;

        }
    }
}
