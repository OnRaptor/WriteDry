using System.Collections.Generic;

namespace WriteDry.Db.Models
{
    public class PCategory
    {
        public int PcategoryId { get; set; }

        public string ProductCategory { get; set; } = null!;

        public virtual ICollection<Product> Products { get; } = new List<Product>();
    }
}
