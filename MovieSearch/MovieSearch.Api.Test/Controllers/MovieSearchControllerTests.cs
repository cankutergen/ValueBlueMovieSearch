using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using MovieSearch.Api.Controllers;
using MovieSearch.Business.Abstract;
using MovieSearch.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Concrete;
using ValueBlue.Core.Utilities.Abstract;

namespace MovieSearch.Api.Test.Controllers
{
    public class MovieSearchControllerTests
    {
        private readonly Mock<IMovieSearchService> movieSearchServiceMock;
        private readonly Mock<ISearchLogService> searchLogServiceMock;
        private readonly Mock<ILogger<MovieSearchController>> loggerMock;
        private readonly Mock<IIpAddressService> ipAddressServiceMock;
        private readonly Mock<IMemoryCache> cacheMock;
        private readonly MovieSearchController controller;

        public MovieSearchControllerTests()
        {
            movieSearchServiceMock = new Mock<IMovieSearchService>();
            searchLogServiceMock = new Mock<ISearchLogService>();
            loggerMock = new Mock<ILogger<MovieSearchController>>();
            ipAddressServiceMock = new Mock<IIpAddressService>();
            cacheMock = new Mock<IMemoryCache>();

            controller = new MovieSearchController(movieSearchServiceMock.Object, searchLogServiceMock.Object, loggerMock.Object, ipAddressServiceMock.Object, cacheMock.Object);
        }


        [Fact]
        public async Task Get_GivenEmtypResult_ShouldReturnNoContentResponse()
        {
            movieSearchServiceMock.Setup(x => x.GetSearchResultByTitleAsync(It.IsAny<string>()));

            var result = await controller.Get("");
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task Get_GivenValidResult_ShouldReturnOkResponse()
        {
            movieSearchServiceMock.Setup(x => x.GetSearchResultByTitleAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new MovieSearchModel()));
            
            ipAddressServiceMock.Setup(x => x.GetIpAddress(It.IsAny<IPAddress>()))
                .Returns("1.1.1.1");

            searchLogServiceMock.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<SearchLogModel>()));

            cacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            var result = await controller.Get("");
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task Get_ThrownException_ShouldReturn500StatusCode()
        {
            movieSearchServiceMock.Setup(x => x.GetSearchResultByTitleAsync(It.IsAny<string>()))
                .Throws(new Exception());

            var result = await controller.Get("");
            ObjectResult actualObjectResult = result as ObjectResult;
            Assert.Equal(500, actualObjectResult.StatusCode);
        }
    }
}
