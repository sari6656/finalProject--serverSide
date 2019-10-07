using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class UserDTO
    {
        public int codeUser { get; set; }
        public string nameUser { get; set; }
        public string phoneUser { get; set; }
        public string mailUser { get; set; }
        public string passwordUser { get; set; }
        public virtual List<SearchDTO> Searches { get; set; }
    }
}
