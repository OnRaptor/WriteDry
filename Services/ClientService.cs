﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
using WriteDry.Models;
using WriteDry.Utils;

namespace WriteDry.Services
{

    public static class CartExtensions
    {
        public static void AddItemToCart(this Cart cart, Cart.CartItem item)
        {
            cart.CartItems.Add(item);
        }

        public static void RemoveItemFromCart(this Cart cart, Cart.CartItem item)
        {
            cart.CartItems.Remove(item);
        }
    }

    public class ClientService
    {
        public User? authorizedUser;
        public bool isGuestEntered;
        public Cart UserCart { get; set; } = new();

        public event EventHandler<AuthArgs> OnAuthStateChanged;

        public class AuthArgs : EventArgs
        {
            public bool isGuest;
            public User? newUserAuth;
            public bool Failed;
            public bool IsAdmin;
            public bool IsSuccesfulRegistration;
        }

        private ApplicationContext db;

        public ClientService(ApplicationContext db)
        {
            this.db = db;
        }

        public void LoginAsGuest()
        {
            isGuestEntered = true;
            OnAuthStateChanged(this, new AuthArgs { isGuest = true });
        }

        public async Task Login(string login, string password)
        {
            var users = await db.Users.ToListAsync();
            foreach (var user in users)
            {
                if (user.UserLogin == login && user.UserPassword == password)
                {
                    authorizedUser = user;
                    isGuestEntered = false;
                    if (user.UserRole == 1 || user.UserRole == 3)
                        OnAuthStateChanged(this, new AuthArgs { newUserAuth = user, IsAdmin = true });
                    else
                        OnAuthStateChanged(this, new AuthArgs { newUserAuth = user });
                    return;
                }
            }
            OnAuthStateChanged(this, new AuthArgs { Failed = true });
        }

        public async Task<int> SubmitOrderAsync(Point PickupPoint, int codeToPickup)
        {
            var order = new Order
            {
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderDeliveryDate = DateTime.Now.AddDays(UserCart.CartItems.FirstOrDefault(a => a.Product.ProductQuantityInStock < 3) != null ? 3 : 6),
                OrderPickupPoint = PickupPoint.PointId,
                OrderFullname = isGuestEntered ? "" : UserFIO.GetFIO(authorizedUser),
                OrderCode = codeToPickup,
                OrderStatus = "Новый"
            };

            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();

            foreach (var cartItem in UserCart.CartItems)
            {
                await db.Orderproducts.AddAsync(new Orderproduct
                {
                    OrderId = order.OrderId,
                    ProductArticleNumber = cartItem.Product.ProductArticleNumber,
                    ProductCount = cartItem.Count
                });
                await db.SaveChangesAsync();
            }
            return order.OrderId;
        }

        public async Task RegisterUser(string UserName, string UsesSurname, string UserPatron, string UserLogin, string UserPassword)
        {
            var alreadyRegisteredUser = db.Users.Any(user => user.UserLogin == UserLogin);
            if (alreadyRegisteredUser) {
                OnAuthStateChanged(this, new()
                {
                    IsSuccesfulRegistration = false
                });
                return;
            }
            var user = new User
            {
                UserName = UserName,
                UserSurname = UsesSurname,
                UserPassword = UserPassword,
                UserPatronymic = UserPatron,
                UserLogin = UserLogin,
                UserRole = 2
            };
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync(true);
            this.authorizedUser = user;
            OnAuthStateChanged(this, new()
            {
                IsSuccesfulRegistration = true,
                newUserAuth = user
            });
        }
    }
}
