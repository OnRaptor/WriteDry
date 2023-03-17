using System.Collections.Generic;

namespace WriteDry.Db.Models
{
    public class Pprovider
    {
        public int PproviderId { get; set; }

        public string ProductProvider { get; set; } = null!;

        public virtual ICollection<Product> Products { get; } = new List<Product>();
    }
}
