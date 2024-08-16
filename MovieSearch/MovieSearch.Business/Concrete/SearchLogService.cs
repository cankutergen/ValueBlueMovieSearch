using MovieSearch.Business.Abstract;
using MovieSearch.Business.Connectivity.Abstract;
using MovieSearch.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearch.Business.Concrete
{
    public class SearchLogService : ISearchLogService
    {
        private readonly ISearchLogApi searchLogApi;

        public SearchLogService(ISearchLogApi searchLogApi)
        {
            this.searchLogApi = searchLogApi;
        }

        public async Task PostAsync(string query, SearchLogModel entity)
        {
            await searchLogApi.PostAsync<SearchLogModel>("/", entity);
        }
    }
}
