using Microsoft.AspNetCore.Identity;
using Core.Entities;

namespace WebUI.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            // Identity yöneticilerini (Manager) servisten çekiyoruz
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            // 1. Rolleri Tanımla
            string adminRoleName = "Admin";
            string userRoleName = "User";

            // Eğer "Admin" rolü yoksa oluştur
            if (!await roleManager.RoleExistsAsync(adminRoleName))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = adminRoleName, Description = "Sistem Yöneticisi" });
            }

            // Eğer "User" rolü yoksa oluştur
            if (!await roleManager.RoleExistsAsync(userRoleName))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = userRoleName, Description = "Standart Kullanıcı" });
            }

            // 2. Admin Kullanıcısını Kontrol Et ve Oluştur
            string adminEmail = "admin@xerp.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null) // Kullanıcı yoksa yeni oluştur
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail, // Giriş yaparken e-posta kullanılacak
                    Email = adminEmail,
                    FirstName = "Sistem",
                    LastName = "Yöneticisi",
                    EmailConfirmed = true // E-posta onayını direkt true yapıyoruz ki onaysız hesaba takılmasın
                };

                // Kullanıcıyı belirlediğimiz şifre ile kaydediyoruz
                var result = await userManager.CreateAsync(adminUser, "Admin123*");

                if (result.Succeeded)
                {
                    // Oluşturulan kullanıcıya "Admin" rolünü ata
                    await userManager.AddToRoleAsync(adminUser, adminRoleName);
                }
            }
        }
    }
}