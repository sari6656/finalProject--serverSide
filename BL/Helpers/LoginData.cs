using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helpers
{
    public class LoginData<T>
    {
        public string TokenJson { get; set; }

        public T objectDTO { get; set; }
    }
}
