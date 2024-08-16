using MovieSearch.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearch.Business.Abstract
{
    public interface ISearchLogService
    {
        Task PostAsync(string query, SearchLogModel entity);
    }
}
