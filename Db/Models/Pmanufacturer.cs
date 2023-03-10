using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteDry.Db.Models
{
    public class PManufacturer
    {
        public int PmanufacturerId { get; set; }

        public string ProductManufacturer { get; set; } = null!;

        public virtual ICollection<Product> Products { get; } = new List<Product>();
    }
}
