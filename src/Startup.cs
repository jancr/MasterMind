using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MasterMind.MMStartup {
    public class Startup {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // services.AddMvc();
            // Add framework services.
            services.AddMvc().AddJsonOptions(options => {
                    //return json format with Camel Case
                    options.SerializerSettings.ContractResolver = 
                        new Newtonsoft.Json.Serialization.DefaultContractResolver();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            // app.UseStaticFiles();
            // app.UseMvc();
            app.UseMvc(routes => {
                routes.MapRoute( "default", "api/{controller=MM}/{action=Show}");
            });
            
        }
    }
}
