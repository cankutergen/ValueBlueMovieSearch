using Admin.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Abstract;
using ValueBlue.Core.Entities.Concrete;

namespace Admin.Business.Abstract
{
    public interface IAdminService
    {
        Task<List<SearchLogModel>> GetAllRecordsAsync(Dictionary<string, string>? authorizationHeader = null, Pagination? pagination = null);

        Task<SearchLogModel> GetSearchLogByIdAsync(string id, Dictionary<string, string>? authorizationHeader = null);

        Task<List<SearchLogModel>> GetSearchLogByDatePeriodAsync(string startDate, string endDate, Dictionary<string, string>? authorizationHeader = null, Pagination? pagination = null);

        Task<DailyUsageReport?> GetDailyUsageReportAsync(string date, Dictionary<string, string>? authorizationHeader = null);

        Task DeleteSearchLogAsync(string id, Dictionary<string, string>? authorizationHeader = null);
    }
}
