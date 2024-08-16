using Microsoft.Extensions.Logging;
using MovieSearch.Business.Connectivity.Abstract;
using MovieSearch.Entities.Concrete;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Concrete;
using ValueBlue.Core.REST.RestSharp;

namespace MovieSearch.Business.Connectivity.Concrete
{
    public class SearchLogApi : RestSharpHttpRepositoryBase, ISearchLogApi
    {
        public SearchLogApi(string baseUrl, ILogger<RestSharpHttpRepositoryBase> logger) : base(baseUrl, logger)
        {
        }
    }
}
