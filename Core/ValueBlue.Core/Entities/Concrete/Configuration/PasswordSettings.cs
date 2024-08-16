using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueBlue.Core.Entities.Concrete.Configuration
{
    public class PasswordSettings
    {
        public int SaltSize { get; set; }

        public int KeySize { get; set; }

        public int Iterations { get; set; }
    }
}
