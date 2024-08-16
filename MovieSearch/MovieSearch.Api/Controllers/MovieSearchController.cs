using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MovieSearch.Api.Configurations;
using MovieSearch.Business.Abstract;
using MovieSearch.Business.Connectivity.Abstract;
using MovieSearch.Entities.Concrete;
using System.Diagnostics;
using System.Net;
using ValueBlue.Core.Entities.Concrete;
using ValueBlue.Core.Utilities.Abstract;

namespace MovieSearch.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieSearchController : ControllerBase
    {
        private readonly IMovieSearchService movieSearchService;
        private readonly ISearchLogService searchLogService;
        private readonly ILogger<MovieSearchController> logger;
        private readonly IIpAddressService ipAddressService;
        private readonly IMemoryCache cache;

        public MovieSearchController(IMovieSearchService movieSearchService, ISearchLogService searchLogService, ILogger<MovieSearchController> logger, IIpAddressService ipAddressService, IMemoryCache cache)
        {
            this.movieSearchService = movieSearchService;
            this.searchLogService = searchLogService;
            this.logger = logger;
            this.ipAddressService = ipAddressService;
            this.cache = cache;
        }

        [HttpGet]
        [Route("Get/{title}")]

        public async Task<IActionResult> Get([FromRoute] string title)
        {
            try
            {
                var watch = Stopwatch.StartNew();
                MovieSearchModel? result = null;

                if (cache.TryGetValue(title, out MovieSearchModel? data))
                {
                    result = data;
                }
                else
                {
                    result = await movieSearchService.GetSearchResultByTitleAsync(title);
                }

                if(result == null)
                {
                    return NoContent();
                }

                cache.Set(title, result, CacheConfiguration.GetCacheOptions());

                watch.Stop();
                var elapsedMiliseconds = watch.ElapsedMilliseconds;

                IPAddress remoteIpAddress = Request?.HttpContext?.Connection?.RemoteIpAddress;
                string ipAddress = ipAddressService.GetIpAddress(remoteIpAddress);

                var searchLogModel = new SearchLogModel
                {
                    ImdbId = result.ImdbID,
                    IpAddress = ipAddress,
                    ProcessingTime = elapsedMiliseconds,
                    SearchToken = title,
                    Timestamp = DateTime.UtcNow
                };

                await searchLogService.PostAsync("/Post", searchLogModel);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);

                return StatusCode(500, ex.Message);
            }
        }
    }
}
