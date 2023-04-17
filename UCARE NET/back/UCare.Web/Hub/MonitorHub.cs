using GQ.Architecture.DDD.Domain.Auth;
using GQ.Log;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using UCare.Shared.Domain.Auth;
using UCare.Shared.Infrastructure.AlertasService;

namespace UCare.Web.Hubs
{
    /// <summary>
    /// 
    /// </summary>gular 
    /// 
    [DisableCors]
    //[SecurityDescription]
    [AllowAnonymous]
    public class MonitorHub : Hub
    {
        private readonly IAuthUserRepository authUser;
        private readonly IAlertasService alertasService;
        private readonly IUniqueSession uniqueSession;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authUser"></param>
        /// <param name="alertasService"></param>
        public MonitorHub(IUniqueSession uniqueSession, IAuthUserRepository authUser, IAlertasService alertasService)
        {
            this.authUser = authUser;
            this.alertasService = alertasService;
            this.uniqueSession = uniqueSession;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            try
            {
                var user = authUser?.GetAuthUser<AuthUser>();
                if (uniqueSession.CheckUser(user!))
                {
                    alertasService.ConnectedMonitor(authUser?.GetAuthUser<AuthUser>()?.Id, this.Context.ConnectionId);
                    Log.Get().Info($"OnConnectedAsync monitoId:{authUser?.GetAuthUser<AuthUser>()?.Id}");
                }
                else
                {
                    Context.Abort();
                }
            }
            catch (Exception exp)
            {
                Log.Get().Error("OnConnectedAsync", exp);
                Context.Abort();
            }
            return base.OnConnectedAsync();
        }

        public bool CheackSession()
        {
            var user = authUser?.GetAuthUser<AuthUser>();
            if (!uniqueSession.CheckUser(user!))
            {
                Context.Abort();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                alertasService.DisconnectedMonitor(authUser.GetAuthUser<AuthUser>().Id, this.Context.ConnectionId);
                if (exception != null)
                    Log.Get().Warn($"OnDisconnectedAsync monitoId:{authUser?.GetAuthUser<AuthUser>()?.Id}", exception);
                Log.Get().Warn($"OnDisconnectedAsync monitoId:{authUser?.GetAuthUser<AuthUser>()?.Id}");
            }
            catch (Exception exp)
            {
                Log.Get().Error("OnDisconnectedAsync", exp);
            }
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsJoin()
        {
            try
            {
                if (!CheackSession()) return false;

                Log.Get().Info($"IsJoin monitoId:{authUser.GetAuthUser<AuthUser>()?.Id}");
                var result = await alertasService.IsJoin(authUser.GetAuthUser<AuthUser>().Id);
                if (result)
                {
                    return await alertasService.AddMonitor(authUser.GetAuthUser<AuthUser>().Id, this.Context.ConnectionId);
                }
                return result;
            }
            catch (Exception exp)
            {
                Log.Get().Error($"IsJoin monitoId:{authUser.GetAuthUser<AuthUser>()?.Id}", exp);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Join()
        {
            try
            {
                if (!CheackSession()) return false;

                Log.Get().Info($"Join monitoId:{authUser.GetAuthUser<AuthUser>()?.Id}");
                return await alertasService.AddMonitor(authUser.GetAuthUser<AuthUser>().Id, this.Context.ConnectionId);
            }
            catch (Exception exp)
            {
                Log.Get().Error($"Join monitoId:{authUser?.GetAuthUser<AuthUser>()?.Id}", exp);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Leave()
        {
            try
            {
                if (!CheackSession()) return false;

                Log.Get().Info($"Leave monitoId:{authUser.GetAuthUser<AuthUser>()?.Id}");
                return await alertasService.RemoveMonitor(authUser.GetAuthUser<AuthUser>().Id);
            }
            catch (Exception exp)
            {
                Log.Get().Error($"Leave monitoId:{authUser.GetAuthUser<AuthUser>()?.Id}", exp);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alertaId"></param>
        /// <returns></returns>
        public async Task<bool> ConfirmarAsignacion(string alertaId)
        {
            try
            {
                if (!CheackSession()) return false;

                Log.Get().Info($"ConfirmarAsignacion monitoId:{authUser.GetAuthUser<AuthUser>()?.Id} alertaId:{alertaId}");
                return await alertasService.ConfirmarAsignacion(authUser.GetAuthUser<AuthUser>().Id, alertaId);
            }
            catch (Exception exp)
            {
                Log.Get().Error($"ConfirmarAsignacion monitoId:{authUser?.GetAuthUser<AuthUser>()?.Id} alertaId:{alertaId}", exp);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alertaId"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public async Task<bool> CambiarEstado(string alertaId, string estado)
        {
            try
            {
                if (!CheackSession()) return false;

                Log.Get().Info($"CambiarEstado monitoId:{authUser.GetAuthUser<AuthUser>()?.Id} alertaId:{alertaId} estado:{estado}");
                return await alertasService.CambiarEstado(authUser.GetAuthUser<AuthUser>().Id, alertaId, estado);
            }
            catch (Exception exp)
            {
                Log.Get().Error($"CambiarEstado monitoId:{authUser.GetAuthUser<AuthUser>()?.Id} alertaId:{alertaId} estado:{estado}", exp);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alertaId"></param>
        /// <param name="bitacora"></param>
        /// <returns></returns>
        public async Task<bool> FinalizarAsistencia(string alertaId, string bitacora)
        {
            try
            {
                if (!CheackSession()) return false;

                Log.Get().Info($"FinalizarAsistencia monitoId:{authUser.GetAuthUser<AuthUser>()?.Id} alertaId:{alertaId}");
                return await alertasService.FinalizarAsistencia(authUser.GetAuthUser<AuthUser>().Id, alertaId, bitacora);
            }
            catch (Exception exp)
            {
                Log.Get().Error($"FinalizarAsistencia monitoId:{authUser.GetAuthUser<AuthUser>()?.Id} alertaId : ${alertaId}", exp);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alertaId"></param>
        /// <returns></returns>
        public async Task<bool> ActivarDesactivarAlarma(string alertaId)
        {
            try
            {
                if (!CheackSession()) return false;

                Log.Get().Info($"ActivarDesactivarAlarma monitoId:{authUser.GetAuthUser<AuthUser>()?.Id} alertaId:{alertaId}");
                return await alertasService.ActivarDesactivarAlarma(authUser.GetAuthUser<AuthUser>().Id, alertaId);
            }
            catch (Exception exp)
            {
                Log.Get().Error($"ActivarDesactivarAlarma monitoId:{authUser.GetAuthUser<AuthUser>()?.Id} alertaId : {alertaId}", exp);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Descanso()
        {
            try
            {
                if (!CheackSession()) return false;

                Log.Get().Info($"Descanso monitoId:{authUser.GetAuthUser<AuthUser>()?.Id}");
                return await alertasService.Descanso(authUser.GetAuthUser<AuthUser>().Id);
            }
            catch (Exception exp)
            {
                Log.Get().Error($"Descanso monitoId:{authUser.GetAuthUser<AuthUser>()?.Id}", exp);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Iniciar()
        {
            try
            {
                if (!CheackSession()) return false;

                Log.Get().Info($"Iniciar monitoId:{authUser.GetAuthUser<AuthUser>()?.Id}");
                return await alertasService.Iniciar(authUser.GetAuthUser<AuthUser>().Id);
            }
            catch (Exception exp)
            {
                Log.Get().Error($"Descanso monitoId:{authUser.GetAuthUser<AuthUser>()?.Id}", exp);
                return false;
            }
        }


    }
}
