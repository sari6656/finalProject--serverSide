using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helpers
{
    public class WebResult<T>
    {
        public bool Status { get; set; }

        public string Message { get; set; }

        public T Value { get; set; }
    }
}
