using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteDry.Db.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string UserSurname { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string UserPatronymic { get; set; } = null!;

        public string UserLogin { get; set; } = null!;

        public string UserPassword { get; set; } = null!;

        public int UserRole { get; set; }

        public virtual Role UserRoleNavigation { get; set; } = null!;
    }
}
