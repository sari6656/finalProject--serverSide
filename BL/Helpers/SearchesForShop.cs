using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helpers
{
    public class SearchesForShop
    {
        public List<string> namesCategories { get; set; }
        public List<int> numbersCategories { get; set; }
        public SearchesForShop()
        {
            namesCategories = new List<string>();
            numbersCategories = new List<int>();
        }
    }
}
