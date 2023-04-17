using GQ.Architecture.DDD.Domain;
using GQ.Architecture.DDD.Domain.Auth;
using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Data.Abstractions.Paging;
using UCare.Application.Alertas;
using UCare.Domain.Alertas;
using UCare.Domain.Users;
using UCare.Shared.Application;
using UCare.Shared.Domain.Auth;
using UCare.Shared.Domain.ValueObjects;
using UCare.Shared.Infrastructure.AlertasService;

namespace UCare.Application.Mapa
{
    public class MapaApp : ApplicationBase
    {
        private readonly IAlertaRepository repository;
        private readonly IUsuarioAfiliadoRepository repositoryAfiliado;
        private readonly IUsuarioManagerRepository usuarioManager;
        private readonly IAlertasService alertasService;
        private readonly AlertasByServiceApp app;
        public MapaApp(AlertasByServiceApp app, IAlertasService alertasService, IAlertaRepository repository, IUsuarioAfiliadoRepository repositoryAfiliado, IUsuarioManagerRepository usuarioManager, IAuthUserRepository authUser, IEventBus eventBus, ICacheService cacheService) : base(authUser, eventBus, cacheService)
        {
            this.repository = repository;
            this.repositoryAfiliado = repositoryAfiliado;
            this.usuarioManager = usuarioManager;
            this.alertasService = alertasService;
            this.app = app;
        }

        public Task<Alerta> GetById(string id)
        {
            return repository.GetById(id);
        }

        public async Task<Shared.Infrastructure.IPaging> GetMonitor(Shared.Infrastructure.IPaging paging)
        {
            paging.Filter = new List<PagingFilter>();

            var user = authUser.GetAuthUser<AuthUser>();
            if (!string.IsNullOrWhiteSpace(user.TenantId))
                paging.Filter.Add(new PagingFilter { Property = "TenantId", Condition = "=", Value = user.TenantId });

            paging.Filter.Add(new PagingFilter { Property = "Cerrado", Condition = "=", Value = false });
            //TODO Se tiene que cambiar por un dia para atras
            paging.Filter.Add(new PagingFilter { Property = "Creado.Modificado", Condition = ">=", Value = DateTime.UtcNow.AddMonths(-1) });
            paging.Filter.Add(new PagingFilter { Property = "Creado.Modificado", Condition = "<=", Value = DateTime.UtcNow.AddDays(1) });

            paging.SetMaxPageSize(int.MaxValue);

            return await repository.Get(paging);
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

            var horaDesde = (long.Parse((paging.Filter.Find(x => x.Property == "desde_time")?.GetValue() ?? 0).ToString())) * TimeSpan.TicksPerMillisecond;
            var horaHasta = (long.Parse((paging.Filter.Find(x => x.Property == "hasta_time")?.GetValue() ?? 0).ToString())) * TimeSpan.TicksPerMillisecond;
            var offset = (long.Parse((paging.Filter.Find(x => x.Property == "offset")?.GetValue() ?? 0).ToString())) * TimeSpan.TicksPerMillisecond;

            var list = result.Data as List<Alerta>;

            if (list != null && list.Count > 0)
            {
                var newList = new List<Alerta>();

                foreach (var item in list)
                {
                    //Filtro de horas
                    if (horaDesde != horaHasta)
                    {
                        var hour = GetTime(item.Creado.Modificado, offset);
                        if (horaDesde > hour || hour > horaHasta)
                        {
                            continue;
                        }
                    }

                    //Filtro de Codigo postal y de edad
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

        private long GetTime(DateTime? dateTime, long offset)
        {
            if (!dateTime.HasValue)
                return -1;
            var ticks = (dateTime.Value.Hour * TimeSpan.TicksPerHour) + (dateTime.Value.Minute * TimeSpan.TicksPerMinute) + (dateTime.Value.Minute * TimeSpan.TicksPerSecond);

            //Correccion de la hora por offset
            // si el offset es de 3 horas 
            // todas las horas que son menor que el offset tiene que pasar para el otro dia
            if (ticks < offset)
            {
                ticks = (24 * TimeSpan.TicksPerHour) + ticks;
            }
            if (offset < 0)
            {
                ticks = ticks - offset;
            }
            return ticks;
        }

        public async Task<bool> SaveConfigMapa(MapaConfig entity)
        {
            var user = authUser.GetAuthUser<AuthUser>();

            return await usuarioManager.UpdateMapaConfig(user.Id, entity);
        }

        public async Task<List<Alerta>> GetAsignados()
        {
            var user = authUser.GetAuthUser<AuthUser>();
            return (await alertasService.GetAlertaByUser(user.Id)).Select(x => x as Alerta).ToList();
        }

        public Task<MapaConfig> GetMapaConfig()
        {
            var user = authUser.GetAuthUser<AuthUser>();
            return GetByParent(user.Id);
        }

        public async Task<MapaConfig> GetByParent(string id)
        {
            var usuario = await usuarioManager.GetById(id);

            if (usuario == null)
                return new MapaConfig();

            if (usuario.Rol != Roles.Monitor && usuario.Mapa != null && usuario.Mapa.Zoom != 0)
                return usuario.Mapa;

            if (string.IsNullOrWhiteSpace(usuario.UsuarioId))
                return new MapaConfig();
            else
                return await GetByParent(usuario.UsuarioId);
        }

        public async Task<UsuarioAfiliado> GetMoreInfo(string alertaId)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var model = await repository.GetById(alertaId);

            if (model == null || (model.Cerrado ?? false) || model.Estado != Estados.Emergencia || model.MonitorId != user.Id)
            {
                throw new Exception("No se pude tener acceso a la informacion del afiliado");
            }

            return await repositoryAfiliado.GetById(model.AfiliadoId);
        }
    }
}
