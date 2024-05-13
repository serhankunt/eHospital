using eHospitalServer.Entities.Enums;
using eHospitalServer.Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace eHospitalServer.WebAPI.Middlewares;

public  class ExtensionsMiddleware
{
    public static void CreateFirstUser(WebApplication app)
    {
        using(var scoped = app.Services.CreateScope())
        {
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<User>>();
            if(!userManager.Users.Any(p=>p.UserName == "admin"))
            {
                User user = new()
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    FirstName = "Hüseyin Serhan",
                    LastName = "Kunt",
                    IdentityNumber = "12345678900",
                    FullAddress = "Ankara",
                    DateOfBirth = DateOnly.Parse("11.09.1996"),
                    EmailConfirmed = true,
                    IsActive = true,
                    IsDeleted = false,
                    BloodType = "A Rh -",
                    UserType = UserType.Admin
                };

                userManager.CreateAsync(user,"1").Wait();
            }
        }
    }
}
