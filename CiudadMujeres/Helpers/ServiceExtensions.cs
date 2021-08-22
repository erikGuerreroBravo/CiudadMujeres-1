using Microsoft.Extensions.DependencyInjection;

namespace CiudadMujeres.Helpers
{
    /// <summary>
    /// Este metodo tiene la finalidad de configurar el cors y su invocación desde metodos remotos desde
    /// servicios con fetch o ajax o axios
    /// </summary>
    public static class ServiceExtensions
    {
           /// <summary>
           /// Este es un metodo de extensión  que se encargará de  configurar mediante un archivo policy las cabeceras cors
           /// </summary>
           /// <param name="services">la interfaz IServiceCollection</param>
            public static void ConfigureCors(this IServiceCollection services)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
                });
            }
        
    }
}
