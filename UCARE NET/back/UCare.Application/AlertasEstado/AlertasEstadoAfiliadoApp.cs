using GQ.Architecture.DDD.Domain;
using GQ.Architecture.DDD.Domain.Auth;
using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Data.Abstractions.Paging;
using Newtonsoft.Json;
using UCare.Application.Alertas;
using UCare.Domain.Alertas;
using UCare.Domain.Users;
using UCare.Shared.Application;
using UCare.Shared.Domain.Auth;
using static UCare.Application.Mapa.MapaApp;

namespace UCare.Application.AlertasEstado
{
    public class AlertasEstadoAfiliadoApp : ApplicationBase
    {
        private readonly IAlertaRepository repository;
        private readonly IUsuarioAfiliadoRepository repositoryAfiliado;
        private readonly AlertasByServiceApp app;

        public AlertasEstadoAfiliadoApp(AlertasByServiceApp app, IAlertaRepository repository, IUsuarioAfiliadoRepository repositoryAfiliado, IAuthUserRepository authUser, IEventBus eventBus, ICacheService cacheService) : base(authUser, eventBus, cacheService)
        {
            this.repository = repository;
            this.repositoryAfiliado = repositoryAfiliado;
            this.app = app;
        }

        public async Task<Shared.Infrastructure.IPaging> Get(Shared.Infrastructure.IPaging paging)
        {
            paging.SetMaxPageSize(int.MaxValue);
            paging.PageSize = paging.GetMaxPageSize();

            var user = authUser.GetAuthUser<AuthUser>();
            if (!string.IsNullOrWhiteSpace(user.TenantId))
                paging.Filter.Add(new PagingFilter { Property = "TenantId", Condition = "=", Value = user.TenantId });

            if (paging.Order.Count == 0)
            {
                paging.Order.Add(new PagingOrder { Property = "Creado.Modificado", Direction = "-" });
            }

            var result = await repository.Get(paging);

            var cp = await app.GetCodigosPostales(user.Id);

            var edadMin = int.Parse((paging.Filter.Find(x => x.Property == "AfiliadoEdadMin")?.GetValue() ?? 0).ToString());
            var edadMax = int.Parse((paging.Filter.Find(x => x.Property == "AfiliadoEdadMax")?.GetValue() ?? 120).ToString());

            var list = result.Data as List<Alerta>;

            if (list != null && list.Count > 0)
            {
                var newList = new List<Alerta>();

                foreach (var item in list)
                {
                    if ((cp == null || cp.Any(x => x == item.AfiliadoCodigoPostal)) &&
                        (edadMin <= 0 || edadMin <= item.AfiliadoEdad) &&
                        (edadMax >= 120 || edadMax >= item.AfiliadoEdad)
                        )
                    {
                        newList.Add(item);
                    }
                }

                result.Data = newList;
            }

            return result;
        }


    }
}
