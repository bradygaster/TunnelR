using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ProductApi
{
    public class Startup
    {
        static string _version = "v1";
        static string _documentName = $"products api ({_version})";

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
                setup.SwaggerDoc(_version, new Microsoft.OpenApi.Models.OpenApiInfo
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
                            ResourceGroupName = "APIs",
                            Version = _version,
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
