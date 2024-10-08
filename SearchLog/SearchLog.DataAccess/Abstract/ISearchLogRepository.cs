﻿using SearchLog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.DataAccess;

namespace SearchLog.DataAccess.Abstract
{
    public interface ISearchLogRepository : IEntityRepository<SearchLogDocument>
    {
        Task<DailyUsageReport?> GetDailyUsageReportAsync(string date);
    }
}
