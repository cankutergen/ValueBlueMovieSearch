using AutoMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SearchLog.DataAccess.Abstract;
using SearchLog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ValueBlue.Core.DataAccess.Mongo.Concrete;
using ValueBlue.Core.Entities.Concrete;

namespace SearchLog.DataAccess.Concrete.Mongo
{
    public class SearchLogRepository : MongoEntityRepositoryBase<SearchLogDocument>, ISearchLogRepository
    {
        private readonly IMapper mapper;

        public SearchLogRepository(IMongoDatabase database, IMapper mapper) : base(database)
        {
            this.mapper = mapper;
        }

        public async Task<DailyUsageReport?> GetDailyUsageReportAsync(string date)
        {
            var startDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            var endDate = startDate.AddDays(1); // Get the next day to use as an exclusive upper bound

            var filter = Builders<SearchLogDocument>.Filter.Gte(r => r.Timestamp, startDate) &
                         Builders<SearchLogDocument>.Filter.Lt(r => r.Timestamp, endDate);

            var aggregation = await collection.Aggregate()
            .Match(filter)
            .Group(new BsonDocument
            {
                    { "_id", new BsonDocument
                        {
                            { "$dateToString", new BsonDocument
                                {
                                    { "format", "%d-%m-%Y" },
                                    { "date", "$Timestamp" }
                                }
                            }
                        }
                    },
                    { "Count", new BsonDocument
                        {
                            { "$sum", 1 }
                        }
                    }
            })
            .ToListAsync();

            var result = aggregation.Select(g => BsonSerializer.Deserialize<AggregationResult>(g)).FirstOrDefault();

            if(result == null)
            {
                return null;
            }

            return mapper.Map<DailyUsageReport>(result);
        }
    }
}
