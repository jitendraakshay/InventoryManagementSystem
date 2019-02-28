using DomainEntities;
using DomainInterface;
using DomainRepository;
using InventoryManagementSystem.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using TicketBookingSystem;

namespace EnventoryManagementSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;          
            DomainRepository.DBManager.DBManager.CS = configuration.GetConnectionString("DefaultConnection");            
            using (IDbConnection connection = DomainRepository.DBManager.DBManager.DbConnect())
            {
                connection.Open();
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ReadConfig>(Configuration.GetSection("ConnectionStrings"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IMenuRepository, MenuRepository>();
            services.AddTransient<IRoleRepo, RoleRepo>();
            services.AddTransient<IAuthorizeUserRepo, AuthorizeUserRepo>();
            services.AddTransient<IUserRepo, UserRepo>();
            services.AddTransient<ILoginUser, LoginUser>();
            services.AddTransient<IDashboardRepo, DashboardRepo>();
            services.AddTransient<ISettingsRepo, SettingsRepo>();
            services.AddTransient<IAuthorizeMenuHelper, AuthorizeMenuHelper>();
            services.AddTransient<IUserProfile, UserProfileRepo>();
;
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
                {
                    config.LoginPath = "/Login/index";
                    config.AccessDeniedPath = "/Login/index";
                    config.Events = new CookieAuthenticationEvents()
                    {
                        OnValidatePrincipal = CookieValidator.ValidateAsync
                    };

                });


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;               
            });

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
                //config.Filters.AddService<SessionExpireAttribute>();

            });            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=controller}/{action=index}/{id?}"
                );


                routes.MapRoute(
                    name: "default",
                    template: "{controller=login}/{action=index}/{id?}"
                    );

            });
        }
    }
}
