using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Misc.Schemes;
using Newtonsoft.Json;

namespace AIR
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
          options.SerializerSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full;
          options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
        });


      services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo() {Title = "AIR API", Version = "v0.1a"}); });

      services.AddAuthentication(options => {
          options.DefaultAuthenticateScheme = "forbidScheme";
          options.DefaultForbidScheme = "forbidScheme";
          options.AddScheme<ForbidScheme>("forbidScheme", "Handle Forbidden");
        }
      );

      // todo inject your logger implementation here
      // services.AddScoped<ILogger>(provider => new MyLogger());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();

        app.UseForwardedHeaders(new ForwardedHeadersOptions {
          ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
      }

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
      // specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
    }
  }
}