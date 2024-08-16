using Microsoft.Extensions.DependencyInjection;
using MovieSearch.Business.Abstract;
using MovieSearch.Business.Concrete;
using MovieSearch.Business.Connectivity.Abstract;
using MovieSearch.Business.Connectivity.Concrete;
using MovieSearch.Entities.Concrete;
using RestSharp;
using Serilog;
using ValueBlue.Core.Entities.Concrete;
using ValueBlue.Core.REST.RestSharp;
using ValueBlue.Core.Utilities.Abstract;
using ValueBlue.Core.Utilities.Concrete;

namespace MovieSearch.Api.Configurations
{
    public static class LoaderModule
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IOmdbApi>(provider =>
                new OmdbApi(builder.Configuration.GetValue<string>("ApiBase:OmdbApi"),
                provider.GetRequiredService<ILogger<RestSharpHttpRepositoryBase>>()));

            builder.Services.AddScoped<ISearchLogApi>(provider =>
                new SearchLogApi(builder.Configuration.GetValue<string>("ApiBase:SearchLogApi"),
                provider.GetRequiredService<ILogger<RestSharpHttpRepositoryBase>>()));

            builder.Services.AddScoped<IMovieSearchService, MovieSearchService>();
            builder.Services.AddScoped<ISearchLogService, SearchLogService>();
            builder.Services.AddScoped<IIpAddressService, IpAddressService>();
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
