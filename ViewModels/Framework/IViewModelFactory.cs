using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteDry.ViewModels.Framework
{
    public interface IViewModelFactory
    {
        AuthViewModel CreateAuthViewModel();
        ListViewModel CreateListViewModel();
        OrderViewModel CreateOrderViewModel();
    }
}
