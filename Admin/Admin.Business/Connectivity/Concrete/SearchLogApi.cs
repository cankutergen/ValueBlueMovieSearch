using Admin.Business.Connectivity.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.REST.RestSharp;

namespace Admin.Business.Connectivity.Concrete
{
    public class SearchLogApi : RestSharpHttpRepositoryBase, ISearchLogApi
    {
        public SearchLogApi(string baseUrl, ILogger<RestSharpHttpRepositoryBase> logger) : base(baseUrl, logger)
        {
        }
    }
}
