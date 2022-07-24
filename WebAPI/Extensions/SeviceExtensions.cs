using System.Text;
using Contracts;
using Entities;
using Entities.Models;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repo;

namespace WebAPI.Extentions
{
    public static class SeviceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(o => { });

        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddScoped<ILoggerManager, LoggerManager>();

        public static void ConfigurePostgres(this IServiceCollection services,
            IConfiguration configuration) =>
            services.AddDbContext<RepoContext>(o => o.UseNpgsql(configuration.GetConnectionString("postgres"),
                o => o.SetPostgresVersion(new Version(9, 6))));

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepoManager, RepoManager>();

        public static void ConfigureIdentity(this IServiceCollection service)
        {
            var builder = service.AddIdentityCore<User>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Password.RequireNonAlphanumeric = true;
                o.Password.RequiredLength = 10;
                o.User.RequireUniqueEmail = true;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<RepoContext>()
                .AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings"); // config at appsetting.json
            // if you use "user secret manager" you can use code below
            //var secretKey = Environment.GetEnvironmentVariable("SECRET");
            var secretKey = jwtSettings.GetSection("SECRET").Value;
            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, // server created the token
                    ValidateAudience = true, // the receiver of the token is a valid recipient
                    ValidateLifetime = true, // token has not expired
                    ValidateIssuerSigningKey = true, // signing key is valid and is trusted be the server
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }
    }
}

