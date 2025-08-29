using Kurochou.Domain.Common;
using Kurochou.Domain.Interface.Repository;
using Kurochou.Domain.Interface.Service;
using Kurochou.Domain.Service;
using Kurochou.Infra.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Data;
using System.Text;

namespace Kurochou.DI;

public static class DependencyInjectionExtension
{
        public static void AddDependencies(this IServiceCollection services, IConfiguration config)
        {
                services.AddServices();
                services.AddJwt(config);
                services.AddControllers();
                services.AddRepositories();
                services.AddDatabase(config);
                services.AddEndpointsApiExplorer();
                services.AddSwagger();
        }

        private static void AddServices(this IServiceCollection services)
        {
                services.AddScoped<ITokenService, TokenService>();
                services.AddScoped<IAuthenticationService, AuthenticationService>();
                services.AddScoped<IUploadService, UploadService>();
                services.AddScoped<IUserService, UserService>();
        }

        private static void AddRepositories(this IServiceCollection services)
        {
                services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IClipRepository, ClipRepository>();
        }

        private static void AddDatabase(this IServiceCollection services, IConfiguration config)
        {
                services.AddScoped<IDbConnection>(_ =>
                {
                        var connectionString = config.GetConnectionString("Default");
                        return new NpgsqlConnection(connectionString);
                });
        }

        private static void AddJwt(this IServiceCollection services, IConfiguration config)
        {
                services.Configure<JwtSettings>(config.GetSection("Jwt"));

                var jwtSettings = config.GetSection("Jwt").Get<JwtSettings>()!;

                services.AddAuthentication(options =>
                {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ValidIssuer = jwtSettings.Issuer,
                                ValidAudience = jwtSettings.Audience,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                        };
                });

                services.AddAuthorizationBuilder()
                        .AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"))
                        .AddPolicy("UserPolicy", policy => policy.RequireRole("User", "Admin"));
        }

        private static void AddSwagger(this IServiceCollection services)
        {
                services.AddSwaggerGen(c =>
                {
                        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kurochou API", Version = "v1" });

                        c.AddSecurityDefinition("Bearer",
                                new OpenApiSecurityScheme
                                {
                                        Name = "Authorization",
                                        Type = SecuritySchemeType.ApiKey,
                                        Scheme = "Bearer",
                                        BearerFormat = "JWT",
                                        In = ParameterLocation.Header,
                                        Description = "JWT Authorization header using the Bearer scheme."
                                });

                        c.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                                {
                                        new OpenApiSecurityScheme
                                        {
                                                Reference = new OpenApiReference
                                                {
                                                        Type = ReferenceType.SecurityScheme,
                                                        Id = "Bearer"
                                                }
                                        },
                                        Array.Empty<string>()
                                }
                        });
                });
        }
}