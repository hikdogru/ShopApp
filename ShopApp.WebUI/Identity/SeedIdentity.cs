using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ShopApp.WebUI.Identity
{
    public static class SeedIdentity
    {
        public static async Task Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            // Appsettings dosyasındaki Data>AdminUser>userName değerine ulaştık
            var userName = configuration["Data:AdminUser:userName"];
            var password = configuration["Data:AdminUser:password"];
            var email = configuration["Data:AdminUser:email"];
            var role = configuration["Data:AdminUser:role"];
            if (await userManager.FindByNameAsync(userName) == null)
            {
                // Admin rolünü oluşturduk. 
                await roleManager.CreateAsync(new IdentityRole(role));

                // Admin kullanıcısını oluşturduk.
                var user = new User()
                {
                    UserName = userName,
                    Email = email,
                    FirstName = "Admin",
                    LastName = "Admin",
                    EmailConfirmed = true
                };
                // Kullanıcıyı veritabanına ekledik.
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    // Kullanıcıya rol atadık.
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}