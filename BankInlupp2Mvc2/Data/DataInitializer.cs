using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankInlupp2Mvc2.Data
{
    public class DataInitializer
    {
        public static void SeedData(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            dbContext.Database.Migrate();
            SeedRoles(dbContext);
            SeedUsers(userManager);
        }

        private static void SeedRoles(ApplicationDbContext dbContext)
        {
            var role = dbContext.Roles.FirstOrDefault(r => r.Name == "Admin");
            if (role == null)
            {
                dbContext.Roles.Add(new IdentityRole { Name = "Admin", NormalizedName = "Admin" });
            }
            role = dbContext.Roles.FirstOrDefault(r => r.Name == "Cashier");
            if (role == null)
            {
                dbContext.Roles.Add(new IdentityRole { Name = "Cashier", NormalizedName = "Cashier" });
            }
            dbContext.SaveChanges();

        }

        private static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            AddUserIfNotExists(userManager, "stefan.holmberg@systementor.se", "Hejsan123#", new string[] { "Admin" });
            AddUserIfNotExists(userManager, "stefan.holmberg@nackademin.se", "Hejsan123#", new string[] { "Cashier" });
        }

        private static void AddUserIfNotExists(UserManager<IdentityUser> userManager,
        string userName, string password, string[] roles)
        {
            if (userManager.FindByEmailAsync(userName).Result != null) return;

            var user = new IdentityUser
            {
                UserName = userName,
                Email = userName,
                EmailConfirmed = true
            };
            var result = userManager.CreateAsync(user, password).Result;
            var r = userManager.AddToRolesAsync(user, roles).Result;
        }
    }
}
