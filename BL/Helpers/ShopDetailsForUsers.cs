using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helpers
{
    public class ShopDetailsForUsers
    {
        public string NameShop { get; set; }
        public string PhoneShop { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string FromHour { get; set; }
        public string ToHour { get; set; }
        public string AddressString { get; set; }
    }
}
