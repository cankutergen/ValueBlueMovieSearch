using MongoDB.Bson;
using SearchLog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLog.Business.Test.Base
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

        public SearchLogDocument GetValidModel()
        {
            return new SearchLogDocument
            {
                Id = new ObjectId(),
                ImdbId = "tt0119094",
                IpAddress = "127.0.0.1",
                ProcessingTime = 590,
                SearchToken = "Face/Off",
                Timestamp = DateTime.Now,
            };
        }
    }
}
