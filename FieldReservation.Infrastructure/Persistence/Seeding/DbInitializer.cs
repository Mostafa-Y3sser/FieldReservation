using FieldReservation.Application.Common.Constants;
using FieldReservation.Domain.Entities;
using FieldReservation.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FieldReservation.Infrastructure.Persistence.Seeding
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Ensure database is created and migrations applied
            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.MigrateAsync();
            }

            // 2. Seed Roles
            string[] roles = { Roles.Player, Roles.Owner };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 3. Seed Default Owner
            var ownerEmail = "admin@gmail.com";
            var existingOwner = await userManager.FindByEmailAsync(ownerEmail);

            if (existingOwner == null)
            {
                var owner = new ApplicationUser
                {
                    FullName = "Field Owner",
                    UserName = "admin",
                    Email = ownerEmail,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.Now
                };

                var result = await userManager.CreateAsync(owner, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(owner, Roles.Owner);
                }
            }
        }
    }
}
