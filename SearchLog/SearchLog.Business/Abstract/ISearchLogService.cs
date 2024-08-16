using SearchLog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Concrete;

namespace SearchLog.Business.Abstract
{
    public interface ISearchLogService
    {
        Task AddSearchLogAsync(SearchLogDocument model);

        Task<List<SearchLogDocument>> GetAllRecordsAsync(Pagination? pagination = null);

        Task<SearchLogDocument> GetSearchLogByIdAsync(string id);

        Task<List<SearchLogDocument>> GetSearchLogByDatePeriodAsync(string startDate, string endDate, Pagination? pagination = null);

        Task<DailyUsageReport?> GetDailyUsageReportAsync(string date);

        Task DeleteSearchLogAsync(string id);
    }
}
