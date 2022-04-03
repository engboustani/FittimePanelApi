using BotDetect.Web;
using FittimePanelApi.Configuration;
using FittimePanelApi.Data;
using FittimePanelApi.INotifications;
using FittimePanelApi.IRepository;
using FittimePanelApi.Notifications;
using FittimePanelApi.Repository;
using FittimePanelApi.Services;
using JSNLog;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FittimePanelApi
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
            services.AddControllers().AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }
            );

            services.AddAuthentication();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteUsers", policy =>
                    policy.RequireRole("Administrator")
                );
                options.AddPolicy("GetAllUsers", policy =>
                    policy.RequireRole("Administrator")
                );
                options.AddPolicy("GetAllTickets", policy =>
                    policy.RequireRole("Administrator")
                );
                options.AddPolicy("GetAllExercises", policy =>
                    policy.RequireRole("Administrator")
                );
                options.AddPolicy("GetAllPayments", policy =>
                    policy.RequireRole("Administrator")
                );
            });

            services.ConfigureIdentity();
            services.ConfigureJWT(Configuration);

            services.AddControllers();

            services.AddCors(o =>
            {
                o.AddPolicy("AllowAll", p =>
                    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );
                o.AddPolicy("Development", p =>
                    p.WithOrigins(origins: new string[] { "http://localhost:8080", "https://localhost:8080" } ).AllowAnyMethod().AllowAnyHeader()
                );
                o.AddPolicy("Production", p =>
                    p.WithOrigins("https://panel.fittimeteam.com").AllowAnyMethod().AllowAnyHeader()
                );
            });

            services.AddAutoMapper(typeof(MapperInitilizer));

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthManager, AuthManager>();

            services.ConfigureRestClient();
            services.ConfigureSmsPanel();
            services.ConfigurePaymentGetaways();
            services.ConfigureURLShortening();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "fittimepanel_api", Version = "v1" });
            });

            string connectionString = ConnectionString();

            services.AddDbContext<AppDb>(
                dbContextOptions => dbContextOptions
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                    .EnableSensitiveDataLogging() // <-- These two calls are optional but help
                    .EnableDetailedErrors()       // <-- with debugging (remove for production).
            );

            // Comment the next line if your app is running on the .NET Core 2.0
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMemoryCache(); // Adds a default in-memory 
                                       // implementation of 
                                       // IDistributedCache

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "fittimepanel_api v1"));
                app.UseCors("AllowAll");
            }

            string connectionString = ConnectionString();
            logger.LogInformation("Connection String: " + connectionString);

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDb>();
                context.Database.Migrate();
            }

            if (env.IsProduction())
            {
                app.UseHttpsRedirection();
            }

            //app.UseCors("AllowAll");
            app.UseCors("Development");
            app.UseCors("Production");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // configure your application pipeline to use SimpleCaptcha middleware
            app.UseSimpleCaptcha(Configuration.GetSection("BotDetect"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                await next();
            });

            var corsAllowedOriginsRegex = @"^http?:\/\/localhost:[0-9]{1,5}\/([-a-zA-Z0-9()@:%_\+.~#?&\/=]*)";
            if (env.IsProduction())
            {
                corsAllowedOriginsRegex = @"^((https?:\/\/)?.*?([\w\d-]*\.[\w\d]+))($|\/.*$)";
            }

            // Configure JSNLog
            var jsnlogConfiguration =
                  new JsnlogConfiguration
                  {
                      corsAllowedOriginsRegex = corsAllowedOriginsRegex,
                  };
            app.UseJSNLog(new LoggingAdapter(loggerFactory), jsnlogConfiguration);
        }

        private string ConnectionString()
        {
            var config = new StringBuilder
                   (Configuration.GetConnectionString("DefaultConnection"));
            return config.Replace("DB_ADDRESS", Configuration["DB_ADDRESS"])
                        .Replace("DB_NAME", Configuration["DB_NAME"])
                        .Replace("DB_USER", Configuration["DB_USER"])
                        .Replace("DB_PASSWORD", Configuration["DB_PASSWORD"])
                        .ToString();
        }
    }
}
