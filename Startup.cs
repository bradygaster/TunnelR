using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ProductApi
{
    public class Startup
    {
        static string _version = "v2";
        static string _documentName = $"Products API ({_version})";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc(_version, new OpenApiInfo
                {
                    Title = _documentName,
                    Version = _version,
                    Description = _documentName
                });
            });
        }

        
        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env,
            IHostApplicationLifetime host
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
                app.UseSwagger(setup => 
                {
                    setup.SerializeAsV2 = true;
                });
                app.UseSwaggerUI(setup => 
                {
                    setup.SwaggerEndpoint($"/swagger/{_version}/swagger.json", _version);
                });

                host.UseApiTestTunnel(app, builder => 
                {
                    builder
                        .UseNGrok()
                        .UseAzureApiMangement(new AzureApiManagementCreateApiOptions
                        {
                            ApiManagementServiceName = "apis",
                            ResourceGroupName = "APIs"
                        });
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
