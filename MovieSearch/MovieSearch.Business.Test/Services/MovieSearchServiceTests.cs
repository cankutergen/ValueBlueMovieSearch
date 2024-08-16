using Microsoft.Extensions.Configuration;
using Moq;
using MovieSearch.Business.Concrete;
using MovieSearch.Business.Connectivity.Abstract;
using MovieSearch.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Concrete;

namespace MovieSearch.Business.Test.Services
{
    public class MovieSearchServiceTests
    {
        private readonly Mock<IOmdbApi> omdbApiMock;
        private readonly Mock<IConfiguration> configurationMock;
        private readonly MovieSearchService movieSearchService;

        public MovieSearchServiceTests()
        {
            omdbApiMock = new Mock<IOmdbApi>();
            configurationMock = new Mock<IConfiguration>();

            movieSearchService = new MovieSearchService(omdbApiMock.Object, configurationMock.Object);
        }

        [Fact]
        public async Task GetSearchResultByTitleAsync_ShouldReturnModel()
        {
            omdbApiMock.Setup(x => x.GetAsync<MovieSearchModel, ErrorModel>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(Task.FromResult(new MovieSearchModel()));

            var result = await movieSearchService.GetSearchResultByTitleAsync("");
            Assert.NotNull(result);
        }
    }
}
