using GQ.Security.Internal;
using Microsoft.AspNetCore.Mvc;
using UCare.Application.Users;
using UCare.Domain.Users;
using UCare.Shared.Domain.ValueObjects;
using UCare.Web.Controllers.Users.Dto;
using UCare.Infrastructure.Firebase;
using UCare.Shared.Infrastructure;
using UCare.Application.Alertas;

namespace UCare.Web.Controllers.Users
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [SecurityDescription("Administracion Usuarios", new object[] { Roles.Propietario, Roles.Gerente, Roles.Administrador })]
    public class UsersController : Controller
    {
        private readonly UsuarioManagerCrudApp app;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public UsersController(UsuarioManagerCrudApp app)
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
        public virtual async Task<IPaging> Find([FromBody] Paging<UsuarioManager, UsuarioManagerDto> paging)
        {
            return await app.Get(paging);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpGet($"api/v1/[controller]/[action]")]
        public virtual async Task<List<UsuarioNameDto>> GetAllUserManager()
        {
            return new UsuarioNameDto().SetEntity(await app.GetAllUserManager());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpGet($"api/v1/[controller]/{{id}}")]
        public virtual async Task<UsuarioManagerDto> GetId(string id)
        {
            return new UsuarioManagerDto().SetEntity(await app.GetById(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpGet($"api/v1/[controller]/Users/{{id}}")]
        public virtual async Task<List<UsuarioManagerDto>> GetUser(string id)
        {
            return new UsuarioManagerDto().SetEntity(await app.GetUsersByUserId(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpGet($"api/v1/[controller]/Replace/{{id}}")]
        public virtual async Task<List<UsuarioManagerDto>> GetReplace(string id)
        {
            return new UsuarioManagerDto().SetEntity(await app.GetUsersByIdRol(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idTo"></param>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpGet($"api/v1/[controller]/ReplaceTo/{{id}}/{{idTo}}")]
        public virtual async Task<bool> GetReplaceTo(string id, string idTo)
        {
            return await app.GetReplaceById(id, idTo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SecurityDescription("Guardar", new object[] { Roles.Propietario, Roles.Gerente, Roles.Administrador })]
        [HttpPut($"api/v1/[controller]")]
        public virtual async Task<UsuarioManagerDto> Save([FromBody] UsuarioManagerDto model)
        {
            var entity = model.GetEntity();
            return model.SetEntity(await app.Save(entity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SecurityDescription("Borrar", new object[] { Roles.Propietario, Roles.Gerente, Roles.Administrador })]
        [HttpDelete($"api/v1/[controller]/{{id}}")]
        public virtual async Task<bool> Delete(string id)
        {
            await app.Delete(id);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpGet($"api/v1/[controller]/[action]")]
        public virtual async Task<List<string>?> CodigosPostales()
        {
            return await app.GetCodigosPostales();
        }
    }
}
