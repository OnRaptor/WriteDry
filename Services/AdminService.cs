using System.Collections.Generic;
using System.Linq;
 

namespace WriteDry.Services
{
    public class AdminService
    {
        public User AuthorizedUser { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();

        private ApplicationContext db;
        public AdminService(ClientService clientService, ApplicationContext applicationContext)
        {
            clientService.OnAuthStateChanged += HandleAuthStateChanged;
            db = applicationContext;
        }

        private void HandleAuthStateChanged(object sender, ClientService.AuthArgs e)
        {
            if (!e.IsAdmin) return;

            AuthorizedUser = e.newUserAuth;
            db.Products.ToList();
            db.Orderproducts.ToList();
            Orders = db.Orders.ToList();
        }
    }
}
