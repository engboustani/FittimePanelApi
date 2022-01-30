using FittimePanelApi.Data;
using FittimePanelApi.Getaways;
using FittimePanelApi.IGetaways;
using FittimePanelApi.INotifications;
using FittimePanelApi.Notifications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FittimePanelApi
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<User>(q => { q.User.RequireUniqueEmail = true; });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<AppDb>().AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration Configuration)
        {
            var jwtSettings = Configuration.GetSection("Jwt");
            var key = Environment.GetEnvironmentVariable("KEY");

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                };
            });
        }

        public static void ConfigureSmsPanel(this IServiceCollection services)
        {
            var username = Environment.GetEnvironmentVariable("SMS_USER");
            var password = Environment.GetEnvironmentVariable("SMS_PASSWORD");
            var from = Environment.GetEnvironmentVariable("SMS_FROM");

            services.AddScoped<IRestClient, RestClient>();
            services.AddScoped<ISmsPanel>(x =>
                new SmsPanel(x.GetRequiredService<IRestClient>(),
                             x.GetRequiredService<ILogger<SmsPanel>>(),
                             username, password, from));
        }

        public static void ConfigureIDPay(this IServiceCollection services)
        {
            var key = Environment.GetEnvironmentVariable("IDPAY_KEY");

            services.AddScoped<IRestClient, RestClient>();
            services.AddScoped<IIDPayGetaway>(x =>
                new IDPayGetaway(x.GetRequiredService<IRestClient>(),
                             x.GetRequiredService<ILogger<IDPayGetaway>>(),
                             key));
        }

        public static void ConfigurePayir(this IServiceCollection services)
        {
            var key = Environment.GetEnvironmentVariable("PAYIR_KEY");

            services.AddScoped<IRestClient, RestClient>();
            services.AddScoped<IPayirGetaway>(x =>
                new PayirGetaway(x.GetRequiredService<IRestClient>(),
                             x.GetRequiredService<ILogger<PayirGetaway>>(),
                             key));
        }

    }
}
