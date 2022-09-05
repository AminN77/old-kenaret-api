using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using BusinessLogic;
using BusinessLogic.Contracts;
using BusinessLogic.Implementation;
using Contracts.Authentication;
using Contracts.FileHandler;
using Contracts.LoggerManager;
using Contracts.Mapper;
using Contracts.OtpHandler;
using Contracts.Repository;
using Infrastructure.Authentication;
using Infrastructure.Data.DataBase;
using Infrastructure.Data.Repository;
using Infrastructure.EventHandlers;
using Infrastructure.FileHandler;
using Infrastructure.Logger;
using Infrastructure.Mapper;
using Infrastructure.OtpHandler;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Kenaret.ActionFilters;

namespace Kenaret.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder
                        .SetIsOriginAllowed(origin => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("X-Pagination")
                        .AllowCredentials());
            });
        }

        public static void ConfigureSqlContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            var host = Environment.GetEnvironmentVariable("PGSQL_HOST");
            var port = Environment.GetEnvironmentVariable("PGSQL_PORT");
            var dataBaseName = "lsec-db";
            var username = Environment.GetEnvironmentVariable("PGSQL_USERNAME");
            var password = Environment.GetEnvironmentVariable("PGSQL_PASSWORD");
            var connectionString = $"Host={host}:{port};Database={dataBaseName};Username={username};password={password}";

            var migration = configuration.GetConnectionString("Docker-PgSqlConnection");

            services.AddDbContext<PgSqlDbContext>(opts =>
                opts.UseNpgsql(connectionString, b =>
                    b.MigrationsAssembly("Infrastructure")));
        }


        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<LogStack>();
            services.AddScoped<ILoggerManager, LoggerManager>();
        }


        public static void AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IUserBusinessLogic, UserBusinessLogic>();
            services.AddScoped<ILinkBusinessLogic, LinkBusinessLogic>();
            services.AddScoped<ILiveStreamBusinessLogic, LiveStreamBusinessLogic>();
            services.AddScoped<IWorldBusinessLogic, WorldBusinessLogic>();
            services.AddScoped<IFeedbackBusinessLogic, FeedbackBusinessLogic>();
            services.AddScoped<IExtensionBusinessLogic, ExtensionBusinessLogic>();
        }

        public static void AddAutomapper(this IServiceCollection services)
        {
            services.AddScoped<IMapper, Mapper>();
        }

        public static void AddJwt(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenCreator, JwtTokenCreator>();
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var secretKey = Encoding.UTF8.GetBytes(configuration["AuthenticationOptions:SecretKey"]);

                    var validationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero, // default: 5 min
                        RequireSignedTokens = true,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                        RequireExpirationTime = true,
                        ValidateLifetime = true,

                        ValidateAudience = true, //default : false
                        ValidAudience = configuration["AuthenticationOptions:Audience"],

                        ValidateIssuer = true, //default : false
                        ValidIssuer = configuration["AuthenticationOptions:Issuer"]
                    };

                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = validationParameters;
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context => Task.CompletedTask,
                        OnChallenge = context => Task.CompletedTask,
                        OnForbidden = context => Task.CompletedTask,
                        OnMessageReceived = context => Task.CompletedTask,
                        OnTokenValidated = context => Task.CompletedTask,
                    };
                });
        }

        public static void AddOtpService(this IServiceCollection services)
        {
            services.AddScoped<IOtpHandler, OtpHandler>();
        }

        public static void AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Presentation", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add accessToken",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer"
                        },
                        new List<string>()
                    }
                });
            });
        }

        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Limit = 120,
                    Period = "1m"
                }
            };

            services.Configure<IpRateLimitOptions>(opt => { opt.GeneralRules = rateLimitRules; });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }

        public static void AddFileService(this IServiceCollection services)
        {
            services.AddScoped<IAvatarHandler, AvatarHandler>();
        }

        public static void ConfigureResponseCaching(this IServiceCollection services)
        {
            services.AddResponseCaching();
        }

        public static void ConfigureCacheHeaders(this IServiceCollection services)
        {
            services.AddHttpCacheHeaders((expirationOpt) =>
                {
                    expirationOpt.MaxAge = 65;
                    expirationOpt.CacheLocation = CacheLocation.Private;
                },
                (validationOpt) => { validationOpt.MustRevalidate = true; });
        }

        public static void ConfigureActionFilters(this IServiceCollection services)
        {
            services.AddScoped<AdminActionFilter>();
        }

        public static void ConfigureEventHandlers(this IServiceCollection services)
        {
            services.AddScoped<EventHandlersManager>();
        }
    }
}