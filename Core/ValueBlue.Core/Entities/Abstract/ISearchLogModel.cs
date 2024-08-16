using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueBlue.Core.Entities.Abstract
{
    public interface ISearchLogModel
    {
        string SearchToken { get; set; }

        string ImdbId { get; set; }

        long ProcessingTime { get; set; }

        DateTime? Timestamp { get; set; }

        string IpAddress {get; set;}
    }
}
