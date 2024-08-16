using FluentValidation;
using SearchLog.Business.Test.Base;
using SearchLog.Business.ValidationRules;
using SearchLog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.CrossCuttingConcerns;

namespace SearchLog.Business.Test.ValidationRules
{
    public class SearchLogValidatorTests : TestBase
    {
        private readonly SearchLogValidator validationRules;

        public SearchLogValidatorTests()
        {
            validationRules = new SearchLogValidator();
            TestModel = GetValidModel();
        }

        [Fact]
        public void Validate_GivenEmptyIpAddress_ShouldReturnErrorList()
        {
            TestModel.IpAddress = string.Empty;

            var result = validationRules.Validate(TestModel);

            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void Validate_GivenEmptySearchToken_ShouldReturnErrorList()
        {
            TestModel.SearchToken = string.Empty;

            var result = validationRules.Validate(TestModel);

            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void Validate_GivenProcessingTimeLowerThanZero_ShouldReturnErrorList()
        {
            TestModel.ProcessingTime = -1;

            var result = validationRules.Validate(TestModel);

            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void Validate_GivenNullTimestamp_ShouldReturnErrorList()
        {
            TestModel.Timestamp = null;

            var result = validationRules.Validate(TestModel);

            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void Validate_GivenNullId_ShouldReturnErrorList()
        {
            TestModel.Id = null;

            var result = validationRules.Validate(TestModel);

            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void Validate_GivenEmptyImdbId_ShouldReturnErrorList()
        {
            TestModel.ImdbId = string.Empty;

            var result = validationRules.Validate(TestModel);

            Assert.NotEmpty(result.Errors);
        }
    }
}
