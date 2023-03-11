using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WriteDry.Db.Models;

namespace WriteDry.Services {
	public class UserService {
		public User? authorizedUser;
		public bool isGuestEntered;

		public event EventHandler<AuthArgs> onAuthChanged;

		public class AuthArgs : EventArgs {
			public bool isGuest;
			public User? newUserAuth;
			public bool Failed;
		}

		private ApplicationContext db;

		public UserService(ApplicationContext db) {
			this.db = db;
		}

		public void LoginAsGuest() {
			isGuestEntered = true;
			onAuthChanged(this, new AuthArgs { isGuest = true });
		}

		public async Task Login(string login, string password) {
			var users = await db.Users.ToListAsync();
			foreach (var user in users) {
				if (user.UserLogin == login && user.UserPassword == password) {
					authorizedUser = user;
					onAuthChanged(this, new AuthArgs { newUserAuth = user });
					return;
				}
			}
			onAuthChanged(this, new AuthArgs { Failed = true });
		}
	}
}
