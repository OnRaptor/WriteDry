using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteDry.Db.Models
{
    public class Point
    {
        public int PointId { get; set; }

        public int Index { get; set; }

        public string City { get; set; } = null!;

        public string Street { get; set; } = null!;

        public int House { get; set; }

        public virtual ICollection<Order> Orders { get; } = new List<Order>();
    }
}
