using Admin.Business.Concrete;
using Admin.Business.Connectivity.Abstract;
using Admin.Business.Test.Base;
using Admin.Entities.Concrete;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Concrete;

namespace Admin.Business.Test.Services
{
    public class AdminServiceTests : TestBase
    {
        private readonly Mock<ISearchLogApi> searchLogApiMock;
        private readonly AdminService adminService;

        public AdminServiceTests()
        {
            searchLogApiMock = new Mock<ISearchLogApi>();
            adminService = new AdminService(searchLogApiMock.Object);
        }

        [Fact]
        public async Task GetAllRecordsAsync_ShouldReturnModelList()
        {
            var data = searchLogApiMock.Setup(x => x.GetAsync<List<SearchLogModel>, ErrorModel>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(Task.FromResult(new List<SearchLogModel> { TestSearchLogModel }));

            var result = await adminService.GetAllRecordsAsync();
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetDailyUsageReportAsync_ShouldReturnModelList()
        {
            var data = searchLogApiMock.Setup(x => x.GetAsync<DailyUsageReport, ErrorModel>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(Task.FromResult(TestDailyUsageReport));

            var result = await adminService.GetDailyUsageReportAsync("");
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetSearchLogByDatePeriodAsync_ShouldReturnModelList()
        {
            var data = searchLogApiMock.Setup(x => x.GetAsync<List<SearchLogModel>, ErrorModel>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(Task.FromResult(new List<SearchLogModel> { TestSearchLogModel }));

            var result = await adminService.GetSearchLogByDatePeriodAsync("", "");
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetSearchLogByIdAsync_ShouldReturnModelList()
        {
            var data = searchLogApiMock.Setup(x => x.GetAsync<SearchLogModel, ErrorModel>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(Task.FromResult(TestSearchLogModel));

            var result = await adminService.GetSearchLogByIdAsync("");
            Assert.NotNull(result);
        }
    }
}
