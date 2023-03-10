using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteDry.Db.Models
{
    public class Pprovider
    {
        public int PproviderId { get; set; }

        public string ProductProvider { get; set; } = null!;

        public virtual ICollection<Product> Products { get; } = new List<Product>();
    }
}
