using Artify.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Artify.Api.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            // Ensure Admin role exists
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            }

            // Check if admin user exists
            var adminEmail = "admin@artify.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Administrator",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }

        public static async Task SeedCategories(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Id = Guid.NewGuid(), Name = "Painting & Drawing" },
                    new Category { Id = Guid.NewGuid(), Name = "Sculpture & Ceramics" },
                    new Category { Id = Guid.NewGuid(), Name = "Textile & Fashion Design" },
                    new Category { Id = Guid.NewGuid(), Name = "Architecture & Interior Design" },
                    new Category { Id = Guid.NewGuid(), Name = "Literature" },
                    new Category { Id = Guid.NewGuid(), Name = "Music" },
                    new Category { Id = Guid.NewGuid(), Name = "Film & Theatre" },
                    new Category { Id = Guid.NewGuid(), Name = "Performing Arts (Dance, Mime, etc.)" },
                    new Category { Id = Guid.NewGuid(), Name = "Digital Art & Graphic Design" },
                    new Category { Id = Guid.NewGuid(), Name = "Decorative Arts & Jewelry" },
                    new Category { Id = Guid.NewGuid(), Name = "Print Making" },
                    new Category { Id = Guid.NewGuid(), Name = "Calligraphy" }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
        }
    }
}
