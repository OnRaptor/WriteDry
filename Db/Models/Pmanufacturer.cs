using System.Collections.Generic;

namespace WriteDry.Db.Models {
	public class PManufacturer {
		public int PmanufacturerId { get; set; }

		public string ProductManufacturer { get; set; } = null!;

		public virtual ICollection<Product> Products { get; } = new List<Product>();
	}
}
