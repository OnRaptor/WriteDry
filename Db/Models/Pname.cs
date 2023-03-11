using System.Collections.Generic;

namespace WriteDry.Db.Models {
	public class Pname {
		public int PnameId { get; set; }

		public string ProductName { get; set; } = null!;

		public virtual ICollection<Product> Products { get; } = new List<Product>();
	}
}
