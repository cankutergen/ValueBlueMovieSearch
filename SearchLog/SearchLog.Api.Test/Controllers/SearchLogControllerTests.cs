using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using SearchLog.Api.Controllers;
using SearchLog.Api.Models;
using SearchLog.Api.Test.Base;
using SearchLog.Business.Abstract;
using SearchLog.Entities.Concrete;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Concrete;

namespace SearchLog.Api.Test.Controllers
{
    public class SearchLogControllerTests : TestBase
    {
        private readonly Mock<ISearchLogService> searchLogServiceMock;
        private readonly Mock<ILogger<SearchLogController>> loggerMock;
        private readonly Mock<IMemoryCache> memoryCacheMock;
        private readonly Mock<IMapper> mapperMock;

        private readonly SearchLogController searchLogController;

        public SearchLogControllerTests()
        {
            searchLogServiceMock = new Mock<ISearchLogService>();
            loggerMock = new Mock<ILogger<SearchLogController>>();
            memoryCacheMock = new Mock<IMemoryCache>();
            mapperMock = new Mock<IMapper>();

            searchLogController = new SearchLogController(searchLogServiceMock.Object, loggerMock.Object, memoryCacheMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task GetAll_GivenEmptyResult_ShouldReturnNoContentResponse()
        {
            var data = new List<SearchLogDocument>();

            searchLogServiceMock.Setup(x => x.GetAllRecordsAsync(It.IsAny<Pagination>()))
                .Returns(Task.FromResult(data));

            var result = await searchLogController.GetAll();
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task GetAll_GivenListResult_ShouldReturnOkResponse()
        {
            var data = new List<SearchLogDocument> { TestModel};

            searchLogServiceMock.Setup(x => x.GetAllRecordsAsync(It.IsAny<Pagination>()))
                .Returns(Task.FromResult(data));

            memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            var result = await searchLogController.GetAll();
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ThrownException_ShouldReturn500StatusCode()
        {
            searchLogServiceMock.Setup(x => x.GetAllRecordsAsync(It.IsAny<Pagination>()))
                .Throws(new Exception());

            var result = await searchLogController.GetAll();
            ObjectResult actualObjectResult = result as ObjectResult;
            Assert.Equal(500, actualObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetById_GivenEmptyResult_ShouldReturnNoContentResponse()
        {
            searchLogServiceMock.Setup(x => x.GetSearchLogByIdAsync(It.IsAny<string>()));

            var result = await searchLogController.GetById("");
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task GetById_GivenResult_ShouldReturnOkResponse()
        {
            var data = TestModel;

            searchLogServiceMock.Setup(x => x.GetSearchLogByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(data));

            memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            var result = await searchLogController.GetById("");
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ThrownArgumentException_ShouldReturnBadRequest()
        {
            searchLogServiceMock.Setup(x => x.GetSearchLogByIdAsync(It.IsAny<string>()))
                .Throws(new ArgumentException());

            var result = await searchLogController.GetById("");
            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ThrownException_ShouldReturn500StatusCode()
        {
            searchLogServiceMock.Setup(x => x.GetSearchLogByIdAsync(It.IsAny<string>()))
                .Throws(new Exception());

            var result = await searchLogController.GetById("");
            ObjectResult actualObjectResult = result as ObjectResult;
            Assert.Equal(500, actualObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetByDatePeriod_GivenEmptyResult_ShouldReturnNoContentResponse()
        {
            var data = new List<SearchLogDocument>();

            searchLogServiceMock.Setup(x => x.GetSearchLogByDatePeriodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>()))
                .Returns(Task.FromResult(data));

            var result = await searchLogController.GetByDatePeriod("", "");
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task GetByDatePeriod_GivenResult_ShouldReturnOkResponse()
        {
            var data = new List<SearchLogDocument> { TestModel };

            searchLogServiceMock.Setup(x => x.GetSearchLogByDatePeriodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>()))
                .Returns(Task.FromResult(data));

            memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            var result = await searchLogController.GetByDatePeriod("", "");
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetByDatePeriod_ThrownArgumentException_ShouldReturBadRequest()
        {
            searchLogServiceMock.Setup(x => x.GetSearchLogByDatePeriodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>()))
                .Throws(new ArgumentException());

            var result = await searchLogController.GetByDatePeriod("", "");
            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }


        [Fact]
        public async Task GetByDatePeriod_ThrownException_ShouldReturn500StatusCode()
        {
            searchLogServiceMock.Setup(x => x.GetSearchLogByDatePeriodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Pagination>()))
                .Throws(new Exception());

            var result = await searchLogController.GetByDatePeriod("", "");
            ObjectResult actualObjectResult = result as ObjectResult;
            Assert.Equal(500, actualObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetDailyUsageReport_GivenEmptyResult_ShouldReturnOkResponse()
        {
            searchLogServiceMock.Setup(x => x.GetDailyUsageReportAsync(It.IsAny<string>()));

            var result = await searchLogController.GetDailyUsageReport("");
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetDailyUsageReport_GivenResult_ShouldReturnOkResponse()
        {
            var data = TestDailyUsageReport;

            searchLogServiceMock.Setup(x => x.GetDailyUsageReportAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(data));

            memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            var result = await searchLogController.GetDailyUsageReport("");
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetDailyUsageReport_ThrownException_ShouldReturn500StatusCode()
        {
            searchLogServiceMock.Setup(x => x.GetDailyUsageReportAsync(It.IsAny<string>()))
                .Throws(new Exception());

            var result = await searchLogController.GetDailyUsageReport("");
            ObjectResult actualObjectResult = result as ObjectResult;
            Assert.Equal(500, actualObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetDailyUsageReport_ThrownArgumentException_ShouldReturnBadRequest()
        {
            searchLogServiceMock.Setup(x => x.GetDailyUsageReportAsync(It.IsAny<string>()))
                .Throws(new ArgumentException());

            var result = await searchLogController.GetDailyUsageReport("");
            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Post_GivenInvalidModel_ShouldReturn422StatusCode()
        {
            var data = TestModel;

            searchLogServiceMock.Setup(x => x.AddSearchLogAsync(It.IsAny<SearchLogDocument>()))
                .Throws(new ValidationException(""));

            mapperMock.Setup(x => x.Map<SearchLogDocument>(It.IsAny<SearchLogRequestModel>()))
                .Returns(data);

            var result = await searchLogController.Post(new SearchLogRequestModel());
            ObjectResult actualObjectResult = result as ObjectResult;
            Assert.Equal(422, actualObjectResult.StatusCode);
        }

        [Fact]
        public async Task Post_GivenValidModel_ShouldReturnCreatedResponse()
        {
            var data = TestModel;

            searchLogServiceMock.Setup(x => x.AddSearchLogAsync(It.IsAny<SearchLogDocument>()));

            mapperMock.Setup(x => x.Map<SearchLogDocument>(It.IsAny<SearchLogRequestModel>()))
                .Returns(data);

            var result = await searchLogController.Post(new SearchLogRequestModel());
            Assert.IsAssignableFrom<CreatedResult>(result);
        }

        [Fact]
        public async Task Delete_GivenNullModel_ShouldReturnNotFoundResponse()
        {
            searchLogServiceMock.Setup(x => x.GetSearchLogByIdAsync(It.IsAny<string>()));

            var result = await searchLogController.Delete("");
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ThrownArgumentException_ShouldReturnBadRequest()
        {
            searchLogServiceMock.Setup(x => x.GetSearchLogByIdAsync(It.IsAny<string>()))
                .Throws(new ArgumentException());

            var result = await searchLogController.Delete("");
            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ThrownException_ShouldReturn500StatusCode()
        {
            searchLogServiceMock.Setup(x => x.GetSearchLogByIdAsync(It.IsAny<string>()))
                .Throws(new Exception());

            var result = await searchLogController.Delete("");
            ObjectResult actualObjectResult = result as ObjectResult;
            Assert.Equal(500, actualObjectResult.StatusCode);
        }

        [Fact]
        public async Task Delete_GivenModel_ShouldReturnOkResponse()
        {
            var data = TestModel;
            searchLogServiceMock.Setup(x => x.GetSearchLogByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(data));

            searchLogServiceMock.Setup(x => x.DeleteSearchLogAsync(It.IsAny<string>()));

            var result = await searchLogController.Delete("");
            Assert.IsAssignableFrom<OkResult>(result);
        }
    }
}
