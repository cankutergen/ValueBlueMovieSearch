using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Abstract;

namespace MovieSearch.Entities.Concrete
{
    public class SearchLogModel : ISearchLogModel
    {
        public string SearchToken { get; set; }

        public string ImdbId { get; set; }

        public long ProcessingTime { get; set; }

        public DateTime? Timestamp { get; set; }

        public string IpAddress { get; set; }
    }
}
