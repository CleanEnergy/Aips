using DAL;
using EntityClasses;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BL.Administrator
{
    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static string GetRoleName(int roleId)
        {
            return roleId == 1 ? "Student" :
                roleId == 2 ? "Administrator" :
                roleId == 3 ? "StudentsOffice" :
                "Professor";
        }

        public static List<Role> GetAll()
        {
            List<Role> roles = new List<Role>();

            roles.Add(new Role { Id = 1, Name = "Student" });
            roles.Add(new Role { Id = 2, Name = "Administrator" });
            roles.Add(new Role { Id = 3, Name = "StudentsOffice" });
            roles.Add(new Role { Id = 4, Name = "Professor" });

            return roles;
        }

        public override string ToString()
        {
            return Name;
        }
    }
    public class Functions
    {
        public ApplicationUserManager UserManager = new ApplicationUserManager(new UserStore<User>(new ApplicationDbContext()));

        public async Task<IdentityResult> CreateUserAccount(User u, string password, string roleId)
        {
            IdentityResult result = await UserManager.CreateAsync(u, password);
            Queries query = new Queries();
            List<IdentityRole> roles = query.GetAllRoles();

            if (result.Succeeded)
            {
                User newUser = await UserManager.FindByNameAsync(u.UserName);
                result = await UserManager.AddToRoleAsync(newUser.Id, roles.Single(x => x.Id == roleId).Name);
                if (!result.Succeeded)
                {
                    await UserManager.DeleteAsync(newUser);
                }
            }

            return result;
        }

        public Task PublishInformation(string title, string content, string userId)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            User admin = context.Users.Where(x => x.Id == userId).Single();
            context.AdminMessages.Add(new AdminMessage
            {
                Content = content,
                Title = title,
                PostedOn = DateTime.Now,
                Admin = admin
            });
            Task.WaitAll(context.SaveChangesAsync());

            return Task.FromResult<object>(null);
        }

        public void LockAccount(string userId)
        {
            UserManager.SetLockoutEnabled(userId, true);
        }

        public void UnlockAccount(string userId)
        {
            UserManager.SetLockoutEnabled(userId, false);
        }

        public async Task<User> FindUserAsync(string userName, string password)
        {
            User userLoggingIn = await UserManager.FindByNameAsync(userName);

            if (userLoggingIn != null)
            {
                if (UserManager.PasswordHasher.VerifyHashedPassword(userLoggingIn.PasswordHash, password) == PasswordVerificationResult.Success)
                {
                    return userLoggingIn;
                }
            }

            return null;
        }

        public Task<ClaimsIdentity> CreateIdentityAsync(User user)
        {
            return UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        }

    }
}
