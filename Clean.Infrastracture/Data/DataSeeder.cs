using Clean.Domain.Enitities;
using Clean.Domain.Entities;

namespace Clean.Infrastracture.Data;

public static class DataSeeder
{
    public static void Seed(DataContext context)
    {
        if (!context.Users.Any(u => u.UserName == "superadmin"))
        {
            context.Users.Add(new User
            {
                UserName = "superadmin",
                FullName = "Super Admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("12345", 11),
                Role = Role.SuperAdmin,
                CompanyId = null
            });
            context.SaveChanges();
        }
    }
}
