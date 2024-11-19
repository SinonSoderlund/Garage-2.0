using Garage_2._0.Data;

namespace Garage_2._0
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<Garage_2_0Context>();

                try
                {
                    await SeedData.InitAsync(context, services);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return app;
        }
    }
}
