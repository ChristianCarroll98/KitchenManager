using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using KitchenManager.API.UsersNS;
using KitchenManager.API.Data;
using KitchenManager.API.Data.Seed;
using KitchenManager.API.ItemsNS.ItemTemplatesNS.Repo;
using KitchenManager.API.ItemsNS.ListItemsNS.Repo;
using KitchenManager.API.ItemTagsNS.Repo;
using KitchenManager.API.UserListsNS.Repo;
using KitchenManager.API.UsersNS.Repo;
using KitchenManager.API.UserAuthNS.Repo;

namespace KitchenManager
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
            services.AddIdentityCore<UserModel>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = System.TimeSpan.FromMinutes(5);
            })
                .AddEntityFrameworkStores<KMDbContext>();

            services.AddDbContext<KMDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("KMData"));
            });

            //services.ConfigureApplicationCookie(options =>
            //{
                
            //});

            services.AddTransient<KMSeeder>();

            services.AddScoped<IItemTemplateRepository, ItemTemplateRepository>();
            services.AddScoped<IListItemRepository, ListItemRepository>();
            services.AddScoped<IItemTagRepository, ItemTagRepository>();
            services.AddScoped<IUserListRepository, UserListRepository>();
            services.AddScoped<IUserAuthenticationRepository, UserAuthenticationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                });

            services
                .AddFluentEmail("defaultsender@test.test")
                .AddSmtpSender("localhost", 25);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(name: "v1", new OpenApiInfo { Title = "KitchenManager API", Version = "v1" });
            });

            services.AddSwaggerGenNewtonsoftSupport();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "KMUI/build";
            });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Kitchen Manager V1.0");
            });

            app.UseHttpsRedirection();
            //app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "KMUI";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
