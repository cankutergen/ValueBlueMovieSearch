using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using RestSharp;
using SearchLog.Business.Abstract;
using SearchLog.Business.Concrete;
using SearchLog.Business.ValidationRules;
using SearchLog.DataAccess.Abstract;
using SearchLog.DataAccess.Concrete.Mongo;
using SearchLog.Entities.Concrete;
using Serilog;
using System.Text;
using ValueBlue.Core.Entities.Concrete.Configuration;

namespace SearchLog.Api.Configurations
{
    public static class LoaderModule
    {
        public static void ConfigureMongo(WebApplicationBuilder builder)
        {
            var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            builder.Services.AddSingleton<IMongoDatabase>(provider =>
            {
                var client = new MongoClient(mongoDbSettings.ConnectionString);
                var databaseName = mongoDbSettings.DatabaseName; // Replace with your MongoDB database name
                return client.GetDatabase(databaseName);
            });
        }

        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped<IValidator<SearchLogDocument>, SearchLogValidator>();
            builder.Services.AddScoped<ISearchLogRepository, SearchLogRepository>();
            builder.Services.AddScoped<ISearchLogService, SearchLogService>();
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
    }
}
