using GQ.Security.Internal;
using Microsoft.AspNetCore.Mvc;
using UCare.Application.AlertasEstado;
using UCare.Domain.Alertas;
using UCare.Infrastructure.Firebase;
using UCare.Shared.Domain.ValueObjects;
using UCare.Shared.Infrastructure;
using UCare.Web.Controllers.AlertasEstado.Dto;

namespace UCare.Web.Controllers.AlertasEstado
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [SecurityDescription("Estados Alertas", new object[] { Roles.Propietario, Roles.Gerente, Roles.Administrador })]
    public class AlertasEstadoController : Controller
    {
        private readonly AlertasEstadoAfiliadoApp app;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public AlertasEstadoController(AlertasEstadoAfiliadoApp app)
        {
            this.app = app;
        }

        [SecurityDescription()]
        [HttpPost($"api/v1/[controller]")]
        public virtual async Task<IPaging> Find([FromBody] Paging<Alerta, AlertaDto> paging)
        {
            return await app.Get(paging);
        }
    }
}
