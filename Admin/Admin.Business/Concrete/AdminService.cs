using Admin.Business.Abstract;
using Admin.Business.Connectivity.Abstract;
using Admin.Entities.Concrete;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Concrete;
using ValueBlue.Core.Utilities.Concrete;

namespace Admin.Business.Concrete
{
    public class AdminService : IAdminService
    {
        private readonly ISearchLogApi searchLogApi;

        public AdminService(ISearchLogApi searchLogApi)
        {
            this.searchLogApi = searchLogApi;
        }

        public async Task DeleteSearchLogAsync(string id, Dictionary<string, string>? authorizationHeader = null)
        {
            await searchLogApi.DeleteAsync($"/Delete/{id}", authorizationHeader);
        }

        public async Task<List<SearchLogModel>> GetAllRecordsAsync(Dictionary<string, string>? authorizationHeader = null, Pagination? pagination = null)
        {
            string query = "/GetAll";

            if (PaginationService.ShouldUsePagination(pagination))
            {
                query = $"{query}?{GetPaginationQuery(pagination)}";
            }

            return await searchLogApi.GetAsync<List<SearchLogModel>, ErrorModel>(query, authorizationHeader);
        }

        public async Task<DailyUsageReport?> GetDailyUsageReportAsync(string date, Dictionary<string, string>? authorizationHeader = null)
        {
            return await searchLogApi.GetAsync<DailyUsageReport, ErrorModel>($"/GetDailyUsageReport/{date}", authorizationHeader);
        }

        public async Task<List<SearchLogModel>> GetSearchLogByDatePeriodAsync(string startDate, string endDate, Dictionary<string, string>? authorizationHeader = null, Pagination? pagination = null)
        {
            string query = $"/GetByDatePeriod?startDate={startDate}&endDate={endDate}";

            if (PaginationService.ShouldUsePagination(pagination))
            {
                query = $"{query}&{GetPaginationQuery(pagination)}";
            }

            return await searchLogApi.GetAsync<List<SearchLogModel>, ErrorModel>(query, authorizationHeader);
        }

        public async Task<SearchLogModel> GetSearchLogByIdAsync(string id, Dictionary<string, string>? authorizationHeader = null)
        {
            return await searchLogApi.GetAsync<SearchLogModel, ErrorModel>($"/GetById/{id}", authorizationHeader);
        }

        private string GetPaginationQuery(Pagination pagination)
        {
            return $"pageSize={pagination.PageSize.ToString()}&page={pagination.Page.ToString()}";
        }
    }
}
