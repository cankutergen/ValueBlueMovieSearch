using Admin.Business.Abstract;
using Admin.Business.Concrete;
using Admin.Business.Connectivity.Abstract;
using Admin.Business.Connectivity.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using ValueBlue.Core.Entities.Concrete.Configuration;
using ValueBlue.Core.REST.RestSharp;

namespace Admin.Api.Configurations
{
    public static class LoaderModule
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<ISearchLogApi>(provider =>
                new SearchLogApi(builder.Configuration.GetValue<string>("ApiBase:SearchLogApi"),
                provider.GetRequiredService<ILogger<RestSharpHttpRepositoryBase>>()));
        }

        public static void ConfigureAuthentication(WebApplicationBuilder builder)
        {
            var jwtTokenSettings = builder.Configuration.GetSection("JwtTokenSettings").Get<JwtTokenSettings>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
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

        public static void ConfigureLogger(WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File("./bin/log/log.txt",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

            builder.Host.UseSerilog(Log.Logger);
        }
    }
}
