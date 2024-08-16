using MovieSearch.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Concrete;
using ValueBlue.Core.REST;

namespace MovieSearch.DataAccess.Abstract
{
    public interface IMovieSearchRepository : IHttpRepository<MovieSearchModel, ErrorModel>
    {
    }
}
