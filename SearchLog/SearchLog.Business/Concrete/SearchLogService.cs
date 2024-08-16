using FluentValidation;
using MongoDB.Bson;
using SearchLog.Business.Abstract;
using SearchLog.DataAccess.Abstract;
using SearchLog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.CrossCuttingConcerns;
using ValueBlue.Core.Entities.Concrete;

namespace SearchLog.Business.Concrete
{
    public class SearchLogService : ISearchLogService
    {
        private readonly ISearchLogRepository repository;
        private readonly IValidator<SearchLogDocument> validator;

        public SearchLogService(ISearchLogRepository repository, IValidator<SearchLogDocument> validator)
        {
            this.repository = repository;
            this.validator = validator;
        }

        public async Task AddSearchLogAsync(SearchLogDocument model)
        {
            if(model.Id == null)
            {
                model.Id = ObjectId.GenerateNewId();
            }

            ValidatorTool.FluentValidate(validator, model);

            await repository.AddAsync(model);
        }

        public async Task DeleteSearchLogAsync(string id)
        {
            if (!IsValidObjectId(id))
            {
                throw new ArgumentException("Given Id is invalid");
            }

            await repository.DeleteAsync(id);
        }

        public async Task<List<SearchLogDocument>> GetAllRecordsAsync(Pagination? pagination = null)
        {
            return await repository.GetAllAsync(pagination);
        }

        public async Task<DailyUsageReport?> GetDailyUsageReportAsync(string date)
        {
            if (!IsValidDate(date))
            {
                throw new ArgumentException("Invalid date format. Please use dd-MM-yyyy.");
            }

            return await repository.GetDailyUsageReportAsync(date);
        }

        public async Task<List<SearchLogDocument>> GetSearchLogByDatePeriodAsync(string startDate, string endDate, Pagination? pagination = null)
        {
            if(!IsValidDate(startDate) || !IsValidDate(endDate))
            {
                throw new ArgumentException("Invalid date format. Please use dd-MM-yyyy.");
            }

            DateTime startDateTime = DateTime.ParseExact(startDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime endDateTime = DateTime.ParseExact(endDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                .AddHours(23)
                .AddMinutes(59)
                .AddSeconds(59);

            if(startDateTime == endDateTime)
            {
                endDateTime = endDateTime.AddDays(1);
            }

            return await repository.GetListAsync(x => x.Timestamp >= startDateTime && x.Timestamp < endDateTime, pagination);
        }

        public async Task<SearchLogDocument> GetSearchLogByIdAsync(string id)
        {
            if (!IsValidObjectId(id))
            {
                throw new ArgumentException("Given Id is invalid");
            }

            var objectId = new ObjectId(id);
            return await repository.GetAsync(x => x.Id == objectId);
        }

        private bool IsValidDate(string date)
        {
            if (!DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var parsedDate))
            {
                return false;
            }

            return true;
        }

        private bool IsValidObjectId(string objectId)
        {
            if(!ObjectId.TryParse(objectId, out var parsedObjectId))
            {
                return false;
            }

            return true;
        }
    }
}
