using FluentValidation;
using SearchLog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLog.Business.ValidationRules
{
    public class SearchLogValidator : AbstractValidator<SearchLogDocument>
    {
        public SearchLogValidator()
        {
            RuleFor(x => x.IpAddress)
                .NotNull()
                .WithMessage("IP address can not be null")
                .NotEmpty()
                .WithMessage("IP address can not be empty");

            RuleFor(x => x.Timestamp)
                .NotNull()
                .WithMessage("Timestamp can not be null");

            RuleFor(x => x.SearchToken)
                .NotNull()
                .WithMessage("Search token can not be null")
                .NotEmpty()
                .WithMessage("Search token can not be empty");

            RuleFor(x => x.ProcessingTime)
                .NotNull()
                .WithMessage("Processing time can not be null")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Processing time should be bigger than 0");

            RuleFor(x => x.Id)
                .NotNull()
                .WithMessage("Id can not be null");

            RuleFor(x => x.ImdbId)
                .NotNull()
                .WithMessage("Imdb Id can not be null")
                .NotEmpty()
                .WithMessage("Imdb Id can not be empty");
        }
    }
}
