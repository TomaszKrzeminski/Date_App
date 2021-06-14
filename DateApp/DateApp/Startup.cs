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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;

namespace DateApp
{
    public class Startup
    {
        /// CORS
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        /// 
        private IHostingEnvironment env;


        private IScheduler _quartzScheduler;
        public Startup(IConfiguration configruation, IHostingEnvironment env)
        {
            Configuration = configruation;
            this.env = env;

            if (env.IsDevelopment())
            {
                _quartzScheduler = ConfigureQuartz();
                //_quartzScheduler = ConfigureQuartzProduction();
            }
            else
            {
                _quartzScheduler = ConfigureQuartzProduction();
            }
            //stop quartz 4

            //

        }

        public IConfiguration Configuration { get; }





        public void ConfigureServices(IServiceCollection services)
        {
            //CORS policy

            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: "_myAllowSpecificOrigins",
            //        builder =>
            //        {
            //            builder.WithOrigins("http://example.com",
            //                                "http://www.contoso.com");
            //        });
            //});




            /// Same Site Flag




            ////

            // Add the cookie to the response cookie collection





            //
            //services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            //


            if (env.IsDevelopment())
            {
                string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=DateAppDevelopment;Trusted_Connection=True;MultipleActiveResultSets=true";
                services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(connectionString));

            }
            else
            {
                services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration["Data:DateAppIdentity:ConnectionString"]));
            }


            //RemoveUser Token

            services.Configure<DataProtectionTokenProviderOptions>(
           x => x.TokenLifespan = TimeSpan.FromMinutes(30));


            services.AddIdentity<AppUser, IdentityRole>()
           .AddEntityFrameworkStores<AppIdentityDbContext>()
           .AddDefaultTokenProviders()
           .AddRemoveUserTotpTokenProvider();

            ///



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




            //stop quartz 1
            //


            services.AddSingleton(provider => _quartzScheduler);

            ///facebook login
            ///


            //services.AddSingleton<IProfileService, ProfileService>();
            //        services.AddAuthentication(options =>
            //        {
            //            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //            options.DefaultSignInScheme = TemporaryAuthenticationDefaults.AuthenticationScheme;
            //            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //        })
            //.AddFacebook(options =>
            //{
            //    options.AppId = "464507421510109";
            //    options.AppSecret = "4749606313209ea08c3c0be5cf372212";
            //})   
            //.AddCookie(options => options.LoginPath = "/account/login")
            //.AddCookie(TemporaryAuthenticationDefaults.AuthenticationScheme);


            //    services
            //.AddAuthentication(o =>
            //{

            //    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddCookie()
            //.AddGoogle(options =>
            //{

            //    options.ClientId = "971555969656-v7va6436ne30770g71soovep0eto6l2q.apps.googleusercontent.com";
            //    options.ClientSecret = "_hWsJTDIUavThkAdJTfYaVOr";
            //    options.UserInformationEndpoint = "https://www.googleapis.com/oauth2/v1/certs";
            //});



            services.AddAuthentication(
                options =>
                {
                    //options.DefaultAuthenticateScheme = IdentityConstants.ExternalScheme;
                    //options.DefaultChallengeScheme = IdentityConstants.ExternalScheme;
                }
                )
            .AddGoogle(o =>
            {
                o.ClientId= Configuration["Authentication:Google:ClientId"];
                o.ClientSecret= Configuration["Authentication:Google:ClientSecret"];
                //o.ClientId = "971555969656-v7va6436ne30770g71soovep0eto6l2q.apps.googleusercontent.com";
                //o.ClientSecret = "_hWsJTDIUavThkAdJTfYaVOr";
                o.UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";
                o.ClaimActions.Clear();
                o.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                o.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                o.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                o.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
                o.ClaimActions.MapJsonKey("urn:google:profile", "link");
                o.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            });






            //options.ClientId = "971555969656-v7va6436ne30770g71soovep0eto6l2q.apps.googleusercontent.com";
            //options.ClientSecret = "_hWsJTDIUavThkAdJTfYaVOr";


            //options.SignInScheme = IdentityConstants.ExternalScheme;
            //options.UserInformationEndpoint = "https://www.googleapis.com/oauth2/v1/certs";



       

            //            { "web":{ project_id":"dateapptest-316109","auth_uri":"https://accounts.google.com/o/oauth2/auth","token_uri":"https://oauth2.googleapis.com/token","auth_provider_x509_cert_url":"https://www.googleapis.com/oauth2/v1/certs","client_secret":"U1rKzh4KpJQzq9LF0_l7rMTj","redirect_uris":["https://localhost:44385/signin-google","https://localhost:44385/Account/GoogleResponse"]
            //    }
            //}




            //     services.AddAuthentication()
            //.AddFacebook(
            //         options =>
            //         {
            //             options.AppId = "464507421510109";
            //             options.AppSecret = "4749606313209ea08c3c0be5cf372212";
            //         }


            //              ).AddCookie(
            //              );








            //     services.AddAuthentication(options =>
            //     {
            //         options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
            //         options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            //         options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            //     })
            //.AddFacebook(
            //         options =>
            //         {
            //             options.AppId = "464507421510109";
            //             options.AppSecret = "4749606313209ea08c3c0be5cf372212";
            //         }


            //         ).AddCookie(
            //         );
            ///

            //        services.AddAuthentication(OAuthValidationDefaults.AuthenticationScheme)
            //.AddOAuthValidation();
            //services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/Login");
            //            services.AddAuthentication(options =>
            //            {
            //                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
            //                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            //                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            //            }).AddFacebook(options =>
            //            {
            //                options.AppId = "464507421510109";
            //                options.AppSecret = "4749606313209ea08c3c0be5cf372212";

            //            });
















            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


        }


        private void OnShutdown()
        {


            if (!_quartzScheduler.IsShutdown) _quartzScheduler.Shutdown(true);


            //stop quartz 2
            //

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AppIdentityDbContext context)
        {






            _quartzScheduler.JobFactory = new AspnetCoreJobFactory(app.ApplicationServices);
            _quartzScheduler.Start().Wait();





            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();






            app.UseStaticFiles();
            ///


            ///



            app.UseAuthentication();

            //




            //

            app.UseSignalR(config => { config.MapHub<UpdatePairHub>("/updatePair"); });
            ////
            app.UseSignalR(config => { config.MapHub<MessageHub>("/messages"); });
            ////
            app.UseSignalR(config => { config.MapHub<CheckConnectionHub>("/messages2"); });
            ////
            app.UseSignalR(config => { config.MapHub<NotificationHub>("/messages3"); });
            /////
            app.UseSignalR(config => { config.MapHub<VideoConnectionHub>("/videos"); });
            /// 
            app.UseSignalR(config => { config.MapHub<NotificationsCheckerHub>("/checknotify"); });
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

            /////
            app.Use(async (ext, next) =>
            {
                var connectionContext = ext.RequestServices
                                        .GetRequiredService<IHubContext<UpdatePairHub>>();
                //...

                if (next != null)
                {
                    await next.Invoke();
                }
            });



            ////
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

            ///////
            app.Use(async (ext, next) =>
            {
                var connectionContext = ext.RequestServices
                                        .GetRequiredService<IHubContext<NotificationsCheckerHub>>();
                //...

                if (next != null)
                {
                    await next.Invoke();
                }
            });
            ///








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
            //app.UseCors(MyAllowSpecificOrigins);
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

        public IScheduler ConfigureQuartzProduction()
        {



            NameValueCollection properties = new NameValueCollection
        {
            { "quartz.scheduler.instanceName", "RemoteServer" },
            { "quartz.scheduler.instanceId", "RemoteServer" },
            { "quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
            { "quartz.jobStore.useProperties", "true" },
            { "quartz.jobStore.dataSource", "default" },
            { "quartz.jobStore.tablePrefix", "QRTZ_" },
            { "quartz.dataSource.default.connectionString",
              "Server=tcp:dateappserver1.database.windows.net,1433;Initial Catalog=QuartzTest2;Persist Security Info=False;User ID=admin1;Password=Martyna1985@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" },
            { "quartz.dataSource.default.provider", "SqlServer" },
            { "quartz.threadPool.threadCount", "1" },
            { "quartz.serializer.type", "json" },
        };

            //    NameValueCollection properties = new NameValueCollection
            //{
            //    { "quartz.scheduler.instanceName", "RemoteServer" },
            //    { "quartz.scheduler.instanceId", "RemoteServer" },
            //    { "quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
            //    { "quartz.jobStore.useProperties", "true" },
            //    { "quartz.jobStore.dataSource", "default" },
            //    { "quartz.jobStore.tablePrefix", "QRTZ_" },
            //    { "quartz.dataSource.default.connectionString",
            //     "Initial Catalog=Quartz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
            //    { "quartz.threadPool.threadCount", "1" },
            //    { "quartz.serializer.type", "json" },
            //};



            StdSchedulerFactory factory = new StdSchedulerFactory(properties);
            var scheduler = factory.GetScheduler().Result;

            //scheduler.ListenerManager.AddTriggerListener(new TriggerListener());
            scheduler.ListenerManager.AddJobListener(new JobListener());
            //scheduler.ListenerManager.AddSchedulerListener(new SchedulerListener());
            return scheduler;

        }





    }
}
