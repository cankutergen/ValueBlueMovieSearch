using Microsoft.Extensions.Configuration;
using MovieSearch.Business.Abstract;
using MovieSearch.Business.Connectivity.Abstract;
using MovieSearch.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Concrete;
using ValueBlue.Core.Entities.Concrete.Configuration;

namespace MovieSearch.Business.Concrete
{
    public class MovieSearchService : IMovieSearchService
    {
        private readonly IOmdbApi omdbApi;
        private readonly IConfiguration configuration;

        public MovieSearchService(IOmdbApi omdbApi, IConfiguration configuration)
        {
            this.omdbApi = omdbApi;
            this.configuration = configuration;
        }

        public async Task<MovieSearchModel> GetSearchResultByTitleAsync(string title)
        {
            string? apiKey = configuration?.GetSection("ApiKeys:OmdbApi")?.Value;

            return await omdbApi.GetAsync<MovieSearchModel, ErrorModel>($"?apiKey={apiKey}&t={title}");
        }
    }
}
