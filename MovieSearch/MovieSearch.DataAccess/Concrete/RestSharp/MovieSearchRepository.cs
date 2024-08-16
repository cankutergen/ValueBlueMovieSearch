using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;
using MovieSearch.DataAccess.Abstract;
using MovieSearch.Entities.Concrete;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Concrete;
using ValueBlue.Core.REST;
using ValueBlue.Core.REST.RestSharp;

namespace MovieSearch.DataAccess.Concrete.RestSharp
{
    public class MovieSearchRepository : RestSharpHttpRepositoryBase<MovieSearchModel, ErrorModel>, IMovieSearchRepository
    {
        public MovieSearchRepository(IRestClient restClient, ILogger<RestSharpHttpRepositoryBase<MovieSearchModel, ErrorModel>> logger) : base(restClient, logger)
        {
        }
    }
}
