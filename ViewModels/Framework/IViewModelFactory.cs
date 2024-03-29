﻿
using WriteDry.ViewModels.Component;

namespace WriteDry.ViewModels.Framework
{
    public interface IViewModelFactory
    {
        AuthViewModel CreateAuthViewModel();
        ListViewModel CreateListViewModel();
        OrderViewModel CreateOrderViewModel();
        OrderItemViewModel CreateOrderItemViewModel();
        AdminShellViewModel CreateAdminShellViewModel();
        RegisterViewModel CreateRegisterViewModel();
        MessageBoxViewModel CreateMessageBoxViewModel();
        EditProductViewModel CreateEditProductViewModel();
        AddProductViewModel CreateAddProductViewModel();
    }
}
