using Admin.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Business.Test.Base
{
    public class TestBase
    {
        protected SearchLogModel TestSearchLogModel;
        protected DailyUsageReport TestDailyUsageReport;

        public TestBase()
        {
            TestSearchLogModel = new SearchLogModel();
            TestDailyUsageReport = new DailyUsageReport();
        }
    }
}
