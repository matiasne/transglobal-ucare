using GQ.Security.Internal;
using Microsoft.AspNetCore.Mvc;
using UCare.Application.Users;
using UCare.Web.Controllers.Mapa.Dto;

namespace UCare.Web.Controllers.Home
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [SecurityDescription()]
    public class HomeController : Controller
    {
        private readonly UsuarioManagerCrudApp app;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public HomeController(UsuarioManagerCrudApp app)
        {
            this.app = app;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpGet("api/v1/[controller]/[action]")]
        public virtual async Task<List<string>?> GetCodigosPostales()
        {
            return await app.GetCodigosPostales();
        }
    }
}
