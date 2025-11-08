using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace CookbookWebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var swaggerProvider = services.GetRequiredService<ISwaggerProvider>();

                var swaggerDoc = swaggerProvider.GetSwagger("v1");
                var json = JsonConvert.SerializeObject(swaggerDoc, Formatting.Indented);
                var path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, "Swagger", "swagger.json");
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                await File.WriteAllTextAsync(path, json);
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}