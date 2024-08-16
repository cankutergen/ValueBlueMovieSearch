using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueBlue.Core.Entities.Abstract
{
    public interface IError
    {
        string? Response { get; set; }

        string? Error { get; set; }
    }
}
