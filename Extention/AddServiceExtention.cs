using Microsoft.AspNetCore.Cors.Infrastructure;
using TimeScaleApi.Service;

namespace WebApi_TimeScale.Extention
{
    public static class AddServiceExtention
    {     
            public static IServiceCollection AddCarService(this IServiceCollection services)
            {
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen(o =>
                {
                    o.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "TimeScale",
                    });

                });

            services.AddScoped<ICSVService, CSVService>();
            return services;
            }
        }
}
