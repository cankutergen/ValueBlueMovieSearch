using FluentValidation;
using Moq;
using SearchLog.Business.Concrete;
using SearchLog.Business.Test.Base;
using SearchLog.DataAccess.Abstract;
using SearchLog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Concrete;

namespace SearchLog.Business.Test.Services
{
    public class SearchLogServiceTests : TestBase
    {
        private readonly Mock<ISearchLogRepository> repositoryMock;
        private readonly Mock<IValidator<SearchLogDocument>> validatorMock;
        private readonly SearchLogService service;

        public SearchLogServiceTests()
        {
            repositoryMock = new Mock<ISearchLogRepository>();
            validatorMock = new Mock<IValidator<SearchLogDocument>>();
            service = new SearchLogService(repositoryMock.Object, validatorMock.Object);
        }

        [Fact]
        public void AddSearchLogAsync_GivenInvalidModel_ShouldThrowValidationException()
        {
            validatorMock.Setup(x => x.Validate(It.IsAny<SearchLogDocument>())).Throws(new ValidationException(""));

            Task result() => service.AddSearchLogAsync(TestModel);

            Assert.ThrowsAsync<ValidationException>(result);
        }

        [Fact]
        public void DeleteSearchLogAsync_GivenInvalidId_ShouldThrowArgumentException()
        {
            Task result() => service.DeleteSearchLogAsync("11");

            Assert.ThrowsAsync<ArgumentException>(result);
        }

        [Fact]
        public async Task GetAllRecordsAsync_ShouldReturnList()
        {
            repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Pagination>()))
                .Returns(Task.FromResult(new List<SearchLogDocument> { TestModel }));

            var result = await service.GetAllRecordsAsync(null);

            Assert.NotEmpty(result);
        }        
        
        [Fact]
        public void GetDailyUsageReportAsync_GivenInvalidDate_ShouldThrowArgumentException()
        {
            Task result() => service.GetDailyUsageReportAsync("invalidate");
            Assert.ThrowsAsync<ArgumentException>(result);
        }

        [Fact]
        public async Task GetDailyUsageReportAsync_GivenValidDate_ShouldReturnReport()
        {
            repositoryMock.Setup(x => x.GetDailyUsageReportAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(TestDailyUsageReport));

            var result = await service.GetDailyUsageReportAsync("15-08-2024");
            Assert.NotNull(result);
        }

        [Fact]
        public void GetSearchLogByDatePeriodAsync_GivenInvalidDates_ShouldThrowArgumentException()
        {
            Task result() => service.GetSearchLogByDatePeriodAsync("08-15-2024", "15-08-2024");
            Assert.ThrowsAsync<ArgumentException>(result);
        }

        [Fact]
        public async Task GetSearchLogByDatePeriodAsync_GivenValidDates_ShouldReturnList()
        {
            repositoryMock.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<SearchLogDocument, bool>>>(), It.IsAny<Pagination>()))
                .Returns(Task.FromResult(new List<SearchLogDocument> { TestModel }));

            var result = await service.GetSearchLogByDatePeriodAsync("14-08-2024", "15-08-2024", null);

            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetSearchLogByIdAsync_GivenInvalidId_ShouldThrowArgumentException()
        {
            Task result() => service.GetSearchLogByIdAsync("11");

            Assert.ThrowsAsync<ArgumentException>(result);
        }

        [Fact]
        public async Task GetSearchLogByIdAsync_GivenValidId_ShouldReturnEntity()
        {
            repositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<SearchLogDocument, bool>>>()))
                .Returns(Task.FromResult(TestModel));

            var result = await service.GetSearchLogByIdAsync("66bca652304113c8005c4ced");

            Assert.NotNull(result);
        }
    }
}
