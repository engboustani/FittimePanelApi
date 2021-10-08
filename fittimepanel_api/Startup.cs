using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
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

namespace fittimepanel_api
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

            var key = "test key for fittime panel";

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSingleton<IJwtAuthenticationManager>(new JwtAuthenticationManager(key));

            services.AddControllers();

            services.AddCors(o =>
            {
                o.AddPolicy("AllowAll", p =>
                    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );
            });

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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "fittimepanel_api v1"));
            }

            string connectionString = ConnectionString();
            logger.LogInformation("Connection String: " + connectionString);

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDb>();
                context.Database.Migrate();
            }


            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
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