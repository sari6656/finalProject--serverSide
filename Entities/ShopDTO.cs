using System.Collections.Generic;
using System.Data.Entity.Spatial;

namespace Entities
{
    public class ShopDTO
    {
        public int codeShop { get; set; }
        public string nameShop { get; set; }
        public string passwordShop { get; set; }
        public string phoneShop { get; set; }
        public string mailShop { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string fromHour { get; set; }
        public string toHour { get; set; }
        public string addressString { get; set; }
        public virtual List<CategoryDTO> Categories { get; set; }

    }
}
