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
using Quartz;
using System.Collections.Specialized;
using Quartz.Impl;
using DateApp.Jobs;
using Microsoft.AspNetCore.Http.Features;

namespace DateApp
{
    public class Startup
    {
        /// CORS
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        /// 



        private IScheduler _quartzScheduler;
        public Startup(IConfiguration configruation)
        {
            Configuration = configruation;
            _quartzScheduler = ConfigureQuartz();

        }

        public IConfiguration Configuration { get; }

       


        public void ConfigureServices(IServiceCollection services)
        {
            //CORS policy

            services.AddCors(options =>
            {
                options.AddPolicy(name: "_myAllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("http://example.com",
                                            "http://www.contoso.com");
                    });
            });



            //
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            //

            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration["Data:DateAppIdentity:ConnectionString"]));

            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
            services.AddTransient<IRepository, Repository>();
            services.AddTransient<ICitiesInRange, CitiesInRangeClass>();
           

            //Quartz Repo
            services.AddTransient<QuartzContext>();
            services.AddTransient<IRepositoryQuartz, RepositoryQuartz>();

            ///ReCapCaptcha
            services.Configure<ReCaptchaSettings>(Configuration.GetSection("GoogleReCaptcha"));
            services.AddTransient<GoogleCaptchaService>();

            //EF
            services.AddMvc()
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            //

            //Invalid logging attemts Brutal Force protection

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            });


            //services.AddMvc();
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();                    


            services.AddTransient<NotificationJob>();            
            services.AddTransient<INotificationsSheduler, NotificationsSheduler>();

            services.AddSingleton(provider => _quartzScheduler);
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


        }


        private void OnShutdown()
        {
            //shutdown quartz is not shutdown already
            //if (!_quartzScheduler.IsShutdown) _quartzScheduler.Shutdown();

            if (!_quartzScheduler.IsShutdown) _quartzScheduler.Shutdown(true);

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AppIdentityDbContext context)
        {

            _quartzScheduler.JobFactory = new AspnetCoreJobFactory(app.ApplicationServices);
            _quartzScheduler.Start().Wait();



            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();


           



            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSignalR(config => { config.MapHub<MessageHub>("/messages"); });
            app.UseSignalR(config => { config.MapHub<CheckConnectionHub>("/messages2"); });
            ////
            app.UseSignalR(config => { config.MapHub<NotificationHub>("/messages3"); });
            /////
            app.UseSignalR(config => { config.MapHub<VideoConnectionHub>("/videos"); });
            /// 
            ///

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

            /////

            app.Use(async (ext, next) =>
            {
                var connectionContext = ext.RequestServices
                                        .GetRequiredService<IHubContext<VideoConnectionHub>>();
                //...

                if (next != null)
                {
                    await next.Invoke();
                }
            });

            ////xss
            //app.Use(async (ctx, next) =>
            //{
            //    ctx.Response.Headers.Add("Content-Security-Policy",
            //                             "default-src 'self'; report-uri /cspreport");
            //    await next();
            //});
            /////
            ///


            ///CORS
            app.UseCors(MyAllowSpecificOrigins);
            ///






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

        public IScheduler ConfigureQuartz()
        {

            //NameValueCollection props = new NameValueCollection
            // {
            //  { "quartz.serializer.type", "json" },             
            //  };

            NameValueCollection properties = new NameValueCollection
        {
            { "quartz.scheduler.instanceName", "RemoteServer" },
            { "quartz.scheduler.instanceId", "RemoteServer" },
            { "quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
            { "quartz.jobStore.useProperties", "true" },
            { "quartz.jobStore.dataSource", "default" },
            { "quartz.jobStore.tablePrefix", "QRTZ_" },
            { "quartz.dataSource.default.connectionString",
              "Server=(localdb)\\MSSQLLocalDB;Database=Quartz;Trusted_Connection=true;" },
            { "quartz.dataSource.default.provider", "SqlServer" },
            { "quartz.threadPool.threadCount", "1" },
            { "quartz.serializer.type", "json" },
        };





            StdSchedulerFactory factory = new StdSchedulerFactory(properties);
            var scheduler = factory.GetScheduler().Result;

            //scheduler.ListenerManager.AddTriggerListener(new TriggerListener());
            scheduler.ListenerManager.AddJobListener(new JobListener());
            //scheduler.ListenerManager.AddSchedulerListener(new SchedulerListener());
            return scheduler;

        }



    }
}
