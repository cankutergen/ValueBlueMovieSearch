using Microsoft.Extensions.Caching.Memory;
using Moq;
using SearchLog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLog.Api.Test.Base
{
    public class TestBase
    {
        protected SearchLogDocument TestModel;
        protected DailyUsageReport? TestDailyUsageReport;

        public TestBase()
        {
            TestModel = new SearchLogDocument();
            TestDailyUsageReport = new DailyUsageReport();
        }
    }
}
