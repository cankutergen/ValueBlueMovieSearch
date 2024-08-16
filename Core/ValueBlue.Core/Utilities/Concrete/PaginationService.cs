using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Concrete;

namespace ValueBlue.Core.Utilities.Concrete
{
    public static class PaginationService
    {
        public static bool ShouldUsePagination(Pagination? pagination)
        {
            return pagination != null && pagination.Page != null && pagination.PageSize != null && pagination.PageSize > 0 && pagination.Page > 0;
        }
    }
}
