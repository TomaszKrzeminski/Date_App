using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using UndergroundSound.Models;
using DateApp.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DateApp
{
    public class Startup
    {

        public Startup(IConfiguration configruation) => Configuration = configruation;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            //

            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration["Data:DateAppIdentity:ConnectionString"]));

            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
            services.AddTransient<IRepository, Repository>();

            //EF
            services.AddMvc()
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            //

            //services.AddMvc();
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AppIdentityDbContext context)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSignalR(config => { config.MapHub<MessageHub>("/messages"); });
            app.UseSignalR(config => { config.MapHub<CheckConnectionHub>("/messages2"); });
            ////
           app.UseSignalR(config => { config.MapHub<NotificationHub>("/messages3"); });

            app.Use(async (ext, next) =>
            {
                var hubContext = ext.RequestServices
                                        .GetRequiredService<IHubContext<MessageHub>>();
                //...

                if (next != null)
                {
                    await next.Invoke();
                }
            });

            app.Use(async (ext, next) =>
            {
                var connectionContext = ext.RequestServices
                                        .GetRequiredService<IHubContext<CheckConnectionHub>>();
                //...

                if (next != null)
                {
                    await next.Invoke();
                }
            });


            //////
            app.Use(async (ext, next) =>
            {
                var connectionContext = ext.RequestServices
                                        .GetRequiredService<IHubContext<NotificationHub>>();
                //...

                if (next != null)
                {
                    await next.Invoke();
                }
            });




            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=StartPage}/{id?}");

                routes.MapRoute(
                    name: "default2",
                    template: "{controller}/{action}/{PairId?}");
            });


            SeedData.EnsurePopulated(context);






        }
    }
}
