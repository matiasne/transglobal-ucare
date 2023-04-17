using GQ.Architecture.DDD;
using GQ.Architecture.DDD.Domain;
using GQ.Architecture.DDD.Domain.Auth;
using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Architecture.DDD.Infrastructure.Auth.Jwt;
using GQ.Architecture.DDD.Infrastructure.Bus.Event;
using GQ.Architecture.DDD.Infrastructure.Cache;
using GQ.Core.service;
using GQ.Log.Log4Net;
using GQ.Security;
using GQ.Security.Jwt;
using GQ.Security.Jwt.SecurityKey;
using GQ.WebApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Common.Middleware.Options;
using System.Reflection;
using UCare.Application.Users;
using UCare.Infrastructure.AlertService;
using UCare.Infrastructure.Firebase;
using UCare.Infrastructure.GoogleApi;
using UCare.Infrastructure.SendinBlue;
using UCare.Infrastructure.SMS.Twilio;
using UCare.Shared.Domain.Auth;
using UCare.Shared.Domain.CodigoPaices;
using UCare.Web.Base;
using UCare.Web.EventsHandlers;
using UCare.Web.Hubs;
using UCare.Web.Repository;

namespace UCare.Web
{
    /// <summary>
    /// https://espressocoder.com/2020/02/05/adding-docker-to-the-asp-net-core-angular-template/
    /// https://referbruv.com/blog/posts/dockerizing-multiple-services-integrating-angular-with-aspnetcore-api-via-docker-compose
    /// https://medium.com/swlh/create-an-asp-net-core-3-0-angular-spa-web-application-with-docker-support-86e8c15796aa
    /// </summary>
    public class Startup : GQStartupWithSpaAngular
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string ContentRootPath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment) : base(configuration, environment)
        {
            AllowAntiforgery = true;
            AllowWebSecurityHeader = false;
            AllowCors = true;
            AllowDataProtection = true;
            UseHttpsRedirection = false;
            AllowSwagger = true;
            ContentRootPath = environment.ContentRootPath;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void BeforeStartLog()
        {
            Log4NetImplement.StartLog();
            Log4NetImplement.UseConsole = true;
            Log4NetImplement.UseTrace = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityConfigure"></param>
        protected override void SecurityConfigureServices(SecurityConfigure securityConfigure)
        {
            securityConfigure.DelegateHasPermission = Base.Security.CheckSecurity;
            securityConfigure.DelegateUsuarioLogueado = SecurityJwt.Usuario<AuthUser>;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwtOptions"></param>
        protected override void JwtConfigureServices(JwtOptions jwtOptions)
        {
            base.JwtConfigureServices(jwtOptions);
            jwtOptions.CookieHeaderName = "jwt";
            jwtOptions.JwtSecurityKey = new SymmetricSecurity(ServicesContainer.Configuration["SecurityKey"] ?? Guid.NewGuid().ToString());
            jwtOptions.TimeOut = TimeSpan.FromHours(6);
            jwtOptions.CookieOptions = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                MaxAge = TimeSpan.FromHours(6)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        protected override void SetControllerOption(MvcOptions options)
        {
            options.Filters.Add<HttpResponseExceptionFilter>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        protected override void ConfigDataBase(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, InMemoryApplicationEventBus>();
            services.AddSingleton<ICacheService, InMemoryCacheService>();
            services.AddSingleton<IUniqueSession, CheckWebUniqueSession>();
            services.AddSingleton<ICodigosPaicesRepositorio, CodigosPaiscesRepository>((sp) =>
            {
                return new CodigosPaiscesRepository(ContentRootPath);
            });

            services.AddAlertService<MonitorHub>(Configuration);
            services.AddHttpContextAccessor();

            services.AddScoped(typeof(IAuthUserRepository), typeof(JwtAuthUserRepository<AuthUser>));

            services.AddApplications(typeof(UsuarioAfiliadoApp).Assembly);

            services.SendinBlue();
            services.AddSMS(Configuration);
            services.AddWebEvents();

            services.AddGoogleApi();

            services.AddDatabase(Configuration, ContentRootPath);

            services.AddSwaggerGen((option) =>
            {
                option.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "UCARE",
                        Description = "Interfaz de apis para sistema UCARE",
                    }
                );
                option.AddSecurityDefinition("jwt", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "jwt",
                    Type = SecuritySchemeType.ApiKey,
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="jwt"
                            }
                        },
                        new string[]{}
                    }
                });

                option.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var condesc = (apiDesc.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor);
                    if ((condesc?.ControllerName?.Contains("Login") ?? false) || (condesc?.ControllerName?.Contains("Mobile") ?? false))
                    {
                        var api = (apiDesc.RelativePath ?? "");
                        return api.Contains(docName) && api.StartsWith("api/");
                    }
                    return false;
                });

                option.EnableAnnotations();

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSignalR(option =>
            {
                option.EnableDetailedErrors = true;
                //option.ClientTimeoutInterval = TimeSpan.FromSeconds(10);
                //option.KeepAliveInterval = TimeSpan.FromSeconds(60);
                //option.HandshakeTimeout = TimeSpan.FromSeconds(10);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurer"></param>
        protected override void ConfigureCsp(IFluentCspOptions configurer)
        {
            base.ConfigureCsp(configurer);

            configurer.ScriptSources(delegate (ICspDirectiveConfiguration s)
            {
                s.Self().UnsafeEval().UnsafeInline().CustomSources("https://maps.googleapis.com");
            });
            configurer.FontSources(delegate (ICspDirectiveBasicConfiguration s)
            {
                s.Self().CustomSources(new string[] { "data:", "https://www.gstatic.com/", "https://fonts.gstatic.com", "https://maps.googleapis.com" });
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoints"></param>
        protected override void UseEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHub<MonitorHub>("/monitor_hub", option =>
            {
                //option.TransportSendTimeout = TimeSpan.FromSeconds(30);
            });
            endpoints.MapFallbackToFile("index.html");
            base.UseEndpoints(endpoints);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="sp"></param>
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider sp)
        {
            app.UseSwagger((option) =>
            {
                option.SerializeAsV2 = true;
                option.RouteTemplate = "swagger/{documentName}/ucare.json";
            });
            app.UseSwaggerUI((option) =>
            {
                option.RoutePrefix = "swagger"; // serve the UI at root
                option.SwaggerEndpoint("v1/ucare.json", "v1");
            });

            base.Configure(app, env, sp);

            using var scope = sp.CreateScope();
            var usuarioApp = scope.ServiceProvider.GetService<UsuarioManagerCrudApp>();
            Base.Security.CreateAccessSecurity(usuarioApp!, this.GetType().Assembly);

        }
    }
}
