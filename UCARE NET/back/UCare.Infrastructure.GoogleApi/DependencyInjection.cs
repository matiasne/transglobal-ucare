using Microsoft.Extensions.DependencyInjection;
using UCare.Shared.Infrastructure.Locations;

namespace UCare.Infrastructure.GoogleApi
{
    public static class DependencyInjection
    {
        public static void AddGoogleApi(this IServiceCollection service)
        {
            service.AddTransient<ILocationRepository, GoogleMapsApi>();
        }
    }
}
