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
using Microsoft.OpenApi.Models;

namespace Inventory.Api {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventory.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(c => {
                c.PreSerializeFilters.Add((swaggerDoc, httpRequest) => {
                    if(!httpRequest.Headers.TryGetValue("X-Forwarded-Host", out var host) || !httpRequest.Headers.TryGetValue("X-Forwarded-Base", out var path))
                        return;

                    var port = $"80";
                    var scheme = "http";

                    if(httpRequest.Headers.TryGetValue("X-Forwarded-Port", out var forwardedPort)) {
                        port = forwardedPort;
                    }

                    if(httpRequest.Headers.TryGetValue("X-Forwarded-Proto", out var forwardedProto)) {
                        scheme = forwardedProto;
                    }

                    var serverUrl = $"{scheme}://" + $"{host}:{port}" + $"{path}";

                    swaggerDoc.Servers = new List<OpenApiServer>()  {
                        new OpenApiServer { Url = serverUrl }
                    };
                });
            });
            app.UseSwaggerUI(c => c.SwaggerEndpoint("../swagger/v1/swagger.json", "Inventory.Api v1"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
