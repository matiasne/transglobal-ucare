using GQ.Core.service;
using GQ.Log;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using UCare.Application.Comunicaciones;
using UCare.Domain.Comunicaciones;
using UCare.Domain.Users;
using UCare.Infrastructure.Firebase;
using UCare.Shared.Domain.ValueObjects;
using UCare.Shared.Infrastructure;

namespace UCare.Comunication.Service
{
    public class Worker : BackgroundService
    {
        public Worker()
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(30000);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = ServicesContainer.CreateScope();

                    ComunicacionApp comunicacionApp = scope.ServiceProvider.GetService<ComunicacionApp>()!;

                    var pendientes = await comunicacionApp.GetPendientes();

                    if (pendientes.Any())
                    {
                        foreach (var comunicacion in pendientes)
                        {
                            List<ComunicacionEnvio> envios = new List<ComunicacionEnvio>();
                            try
                            {
                                IPaging page = null;
                                page = JsonConvert.DeserializeObject<Paging<UsuarioAfiliado>>(comunicacion.Destinos)!;
                                page = await comunicacionApp.GetAllAfiliados(page!);

                                if (page != null)
                                {
                                    var afiliados = page.Data.Cast<UsuarioAfiliado>();
                                    if (afiliados.Any())
                                    {
                                        foreach (var afilado in afiliados)
                                        {
                                            envios.Add(new ComunicacionEnvio
                                            {
                                                ComunicadoId = await comunicacionApp.AfiliadoAddComunicacion(afilado, comunicacion),
                                                AfiliadoId = afilado.Id!,
                                                Estado = Estados.NoLeido
                                            });
                                        }
                                        comunicacion.DetalleEnvio = "Notificacion enviada correctamente";
                                        comunicacion.Enviado = true;
                                        comunicacion.Estado = "A";
                                    }
                                    else
                                    {
                                        comunicacion.DetalleEnvio = "No se encontraron afiliaos para enviar la notificacion";
                                        comunicacion.Enviado = true;
                                        comunicacion.Estado = "E";
                                    }
                                }
                                else
                                {
                                    comunicacion.DetalleEnvio = "La Pagina de afiliados retorno null";
                                    comunicacion.Enviado = true;
                                    comunicacion.Estado = "E";
                                }
                            }
                            catch (Exception ex)
                            {
                                comunicacion.DetalleEnvio = ex.Message;
                                comunicacion.Enviado = true;
                                comunicacion.Estado = "E";
                            }
                            finally
                            {
                                await comunicacionApp.UpadeteEstado(comunicacion, envios);
                            }
                        }
                    }
                }
                catch (Exception generalEx)
                {
                    Log.Get().Error("ExecuteAsync", generalEx);
                }
                finally
                {
                    await Task.Delay(60000, stoppingToken);

                }
            }
        }
    }
}