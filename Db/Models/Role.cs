using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteDry.Db.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; } = null!;

        public virtual ICollection<User> Users { get; } = new List<User>();
    }
}
