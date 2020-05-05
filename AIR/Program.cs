using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AIR
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => {
          webBuilder
            .ConfigureAppConfiguration((hostingContext, config) => {
              config.AddJsonFile(
                "Configs/secrets.json",
                optional: false,
                reloadOnChange: false
              );
            })
            .UseUrls("http://0.0.0.0:51228")
            .UseStartup<Startup>();
        });
  }
}