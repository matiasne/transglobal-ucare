using GQ.Core.service;
using UCare.Comunication.Service;
using UCare.Web;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.UseStartup<Startup>();
})
.ConfigureServices((hostContext, services) =>
 {
     services.AddHostedService<Worker>();
 });

ServicesContainer.AddHost(builder.Build());
ServicesContainer.Host.Run();
