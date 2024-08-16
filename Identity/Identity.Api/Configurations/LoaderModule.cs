using FluentValidation;
using Identity.Business.Abstract;
using Identity.Business.Concrete;
using Identity.Business.ValidationRules;
using Identity.DataAccess.Abstract;
using Identity.DataAccess.Concrete.Mongo;
using Identity.Entities.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using System.Text;
using ValueBlue.Core.Entities.Concrete.Configuration;

namespace Identity.Api.Configurations
{
    public static class LoaderModule
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
            builder.Services.AddScoped<IValidator<UserDocument>, UserValidator>();
            builder.Services.AddScoped<ITokenService, JwtTokenService>();
            builder.Services.AddScoped<IPasswordService, PasswordService>();
            builder.Services.AddScoped<IIdentityService, IdentityService>();
        }

        public static void ConfigureJwtService(WebApplicationBuilder builder)
        {
            var jwtTokenSettings = builder.Configuration.GetSection("JwtTokenSettings").Get<JwtTokenSettings>();

            var key = Encoding.ASCII.GetBytes(jwtTokenSettings.Key);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtTokenSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            builder.Services.AddAuthorization();
        }

        public static void ConfigureMongo(WebApplicationBuilder builder)
        {
            var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            builder.Services.AddSingleton<IMongoDatabase>(provider =>
            {
                var client = new MongoClient(mongoDbSettings.ConnectionString);
                var databaseName = mongoDbSettings.DatabaseName;
                return client.GetDatabase(databaseName);
            });
        }

        public static void ConfigureLogger(WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File("./bin/log/log.txt",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

            builder.Host.UseSerilog(Log.Logger);
        }

        public static void ConfigureAuthentication(WebApplicationBuilder builder)
        {
            var jwtTokenSettings = builder.Configuration.GetSection("JwtTokenSettings").Get<JwtTokenSettings>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("JwtBearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtTokenSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSettings.Key)),
                    ClockSkew = TimeSpan.Zero
                };
            });        
        }

        public static void ConfigureSwagger(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                // Define the OAuth2.0 scheme that's in use (i.e., Implicit Flow)
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }

        public static async Task CreateDefaultAdminUserAsync(WebApplication app, WebApplicationBuilder builder)
        {
            using (var scope = app.Services.CreateScope())
            {
                var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();
                var adminUserSettings = builder.Configuration.GetSection("AdminUserSettings").Get<AdminUserSettings>();

                var user = new UserDocument
                {
                    Id = ObjectId.GenerateNewId(),
                    Password = adminUserSettings.Password,
                    Username = adminUserSettings.Username,
                    Role = new UserRole
                    {
                        Id = ObjectId.GenerateNewId(),
                        RoleName = adminUserSettings.Role,
                    }
                };

                if(!await identityService.IsUserExists(user.Username))
                {
                    await identityService.RegisterUserAsync(user);
                }
            }
        }
    }
}
