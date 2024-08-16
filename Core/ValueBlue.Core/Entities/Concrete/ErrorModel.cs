using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Abstract;

namespace ValueBlue.Core.Entities.Concrete
{
    public class ErrorModel : IError
    {
        public string? Response { get; set; }

        public string? Error { get; set; }
    }
}
