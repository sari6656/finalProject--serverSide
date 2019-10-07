using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class SearchDTO
    {
        public int codeSearch { get; set; }
        public int codeUser { get; set; }
        public string nameProduct { get; set; }
        public int status { get; set; }
        public int? codeShop { get; set; }
        public int codeCategory { get; set; }

    }
}
