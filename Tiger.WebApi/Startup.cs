using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Tiger.WebApi.Core;
using Tiger.WebApi.Core.Extensions;

namespace Tiger.WebApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.SettingsTigerWebApi();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.Map("/info", ApiHandler.Info);
            app.Map("/rest", ApiHandler.Map);

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello TigerWebApi!");
            });
        }
    }
}
