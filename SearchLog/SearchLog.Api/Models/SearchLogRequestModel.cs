using ValueBlue.Core.Entities.Abstract;

namespace SearchLog.Api.Models
{
    public class SearchLogRequestModel : ISearchLogModel
    {
        public string SearchToken { get; set; }

        public string ImdbId { get; set; }

        public long ProcessingTime { get; set; }

        public DateTime? Timestamp { get; set; }

        public string IpAddress { get; set; }
    }
}
