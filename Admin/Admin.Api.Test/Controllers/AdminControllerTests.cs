using Admin.Api.Controllers;
using Admin.Api.Test.Base;
using Admin.Business.Abstract;
using Admin.Entities.Concrete;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Concrete;

namespace Admin.Api.Test.Controllers
{
    public class AdminControllerTests : TestBase
    {
        private readonly Mock<IAdminService> adminServiceMock;
        private readonly Mock<ILogger<AdminController>> loggerMock;
        private readonly Mock<IMemoryCache> memoryCacheMock;
        private readonly AdminController adminController;

        public AdminControllerTests()
        {
            adminServiceMock = new Mock<IAdminService>();
            loggerMock = new Mock<ILogger<AdminController>>();
            memoryCacheMock = new Mock<IMemoryCache>();

            adminController = new AdminController(adminServiceMock.Object, loggerMock.Object, memoryCacheMock.Object);
        }

        [Fact]
        public async Task GetAll_GivenEmptyResult_ShouldReturnNoContentResponse()
        {
            var data = new List<SearchLogModel>();

            adminServiceMock.Setup(x => x.GetAllRecordsAsync(It.IsAny<Dictionary<string, string>>(), It.IsAny<Pagination>()))
                .Returns(Task.FromResult(data));

            var result = await adminController.GetAll();
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task GetAll_GivenListResult_ShouldReturnOkResponse()
        {
            var data = new List<SearchLogModel> { TestSearchLogModel };

            adminServiceMock.Setup(x => x.GetAllRecordsAsync(It.IsAny<Dictionary<string, string>>(), It.IsAny<Pagination>()))
                .Returns(Task.FromResult(data));

            memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            var result = await adminController.GetAll();
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ThrownException_ShouldReturn500StatusCode()
        {
            adminServiceMock.Setup(x => x.GetAllRecordsAsync(It.IsAny<Dictionary<string, string>>(), It.IsAny<Pagination>()))
                .Throws(new Exception());

            var result = await adminController.GetAll();
            ObjectResult actualObjectResult = result as ObjectResult;
            Assert.Equal(500, actualObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetById_GivenEmptyResult_ShouldReturnNoContentResponse()
        {
            adminServiceMock.Setup(x => x.GetSearchLogByIdAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            var result = await adminController.GetById("");
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task GetById_GivenResult_ShouldReturnOkResponse()
        {
            adminServiceMock.Setup(x => x.GetSearchLogByIdAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(Task.FromResult(TestSearchLogModel));

            memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            var result = await adminController.GetById("");
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ThrownArgumentException_ShouldReturnBadRequest()
        {
            adminServiceMock.Setup(x => x.GetSearchLogByIdAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Throws(new ArgumentException());

            var result = await adminController.GetById("");
            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ThrownException_ShouldReturn500StatusCode()
        {
            adminServiceMock.Setup(x => x.GetSearchLogByIdAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Throws(new Exception());

            var result = await adminController.GetById("");
            ObjectResult actualObjectResult = result as ObjectResult;
            Assert.Equal(500, actualObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetByDatePeriod_GivenEmptyResult_ShouldReturnNoContentResponse()
        {
            var data = new List<SearchLogModel>();

            adminServiceMock.Setup(x => x.GetSearchLogByDatePeriodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Pagination>()))
                .Returns(Task.FromResult(data));

            var result = await adminController.GetByDatePeriod("", "");
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task GetByDatePeriod_GivenResult_ShouldReturnOkResponse()
        {
            var data = new List<SearchLogModel> { TestSearchLogModel };

            adminServiceMock.Setup(x => x.GetSearchLogByDatePeriodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Pagination>()))
                .Returns(Task.FromResult(data));

            memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            var result = await adminController.GetByDatePeriod("", "");
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetByDatePeriod_ThrownException_ShouldReturn500StatusCode()
        {
            adminServiceMock.Setup(x => x.GetSearchLogByDatePeriodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Pagination>()))
                .Throws(new Exception());

            var result = await adminController.GetByDatePeriod("", "");
            ObjectResult actualObjectResult = result as ObjectResult;
            Assert.Equal(500, actualObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetByDatePeriod_ThrownArgumentException_ShouldReturn500StatusCode()
        {
            adminServiceMock.Setup(x => x.GetSearchLogByDatePeriodAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Pagination>()))
                .Throws(new ArgumentException());

            var result = await adminController.GetByDatePeriod("", "");
            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetDailyUsageReport_GivenEmptyResult_ShouldReturnOkResponse()
        {
            adminServiceMock.Setup(x => x.GetDailyUsageReportAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            var result = await adminController.GetDailyUsageReport("");
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task GetDailyUsageReport_GivenResult_ShouldReturnOkResponse()
        {
            var data = TestDailyUsageReport;

            adminServiceMock.Setup(x => x.GetDailyUsageReportAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(Task.FromResult(data));

            memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            var result = await adminController.GetDailyUsageReport("");
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetDailyUsageReport_ThrownException_ShouldReturn500StatusCode()
        {
            adminServiceMock.Setup(x => x.GetDailyUsageReportAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Throws(new Exception());

            var result = await adminController.GetDailyUsageReport("");
            ObjectResult actualObjectResult = result as ObjectResult;
            Assert.Equal(500, actualObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetDailyUsageReport_ThrownArgumentException_ShouldReturnBadRequest()
        {
            adminServiceMock.Setup(x => x.GetDailyUsageReportAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Throws(new ArgumentException());

            var result = await adminController.GetDailyUsageReport("");
            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ThrownArgumentException_ShouldReturnBadRequest()
        {
            adminServiceMock.Setup(x => x.DeleteSearchLogAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Throws(new ArgumentException());

            var result = await adminController.Delete("");
            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ThrownException_ShouldReturn500StatusCode()
        {
            adminServiceMock.Setup(x => x.DeleteSearchLogAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Throws(new Exception());

            var result = await adminController.Delete("");
            ObjectResult actualObjectResult = result as ObjectResult;
            Assert.Equal(500, actualObjectResult.StatusCode);
        }

        [Fact]
        public async Task Delete_GivenModel_ShouldReturnOkResponse()
        {
            var data = TestSearchLogModel;
            adminServiceMock.Setup(x => x.DeleteSearchLogAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(Task.FromResult(data));

            var result = await adminController.Delete("");
            Assert.IsAssignableFrom<OkResult>(result);
        }
    }
}
