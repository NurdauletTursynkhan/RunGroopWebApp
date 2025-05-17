using Microsoft.AspNetCore.Identity;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                if (context == null) return;

                context.Database.EnsureCreated();

                if (!context.Clubs.Any())
                {
                    context.Clubs.AddRange(new List<Club>()
                    {
                        new Club
                        {
                            Title = "Running Club 1",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg",
                            Description = "Description for Club 1",
                            ClubCategory = ClubCategory.City,
                            Address = new Address { Street = "Street 1", City = "Almaty", State = "KZ" }
                        },
                        new Club
                        {
                            Title = "Running Club 2",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg",
                            Description = "Description for Club 2",
                            ClubCategory = ClubCategory.Endurance,
                            Address = new Address { Street = "Street 2", City = "Astana", State = "KZ" }
                        }
                    });
                    context.SaveChanges();
                }

                if (!context.Races.Any())
                {
                    context.Races.AddRange(new List<Race>()
                    {
                        new Race
                        {
                            Title = "Race 1",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg",
                            Description = "Description for Race 1",
                            RaceCategory = RaceCategory.Marathon,
                            Address = new Address { Street = "Street 3", City = "Shymkent", State = "KZ" }
                        }
                    });
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                // Create Roles
                var roles = new[] { "Admin", "User" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }

                // Create Admin
                var adminEmail = "admin@rungroop.kz";
                var adminPassword = "Admin@1234";

                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var adminUser = new AppUser
                    {
                        UserName = "admin",
                        Email = adminEmail,
                        EmailConfirmed = true,
                        Address = new Address { Street = "Admin Street", City = "Almaty", State = "KZ" }
                    };
                    await userManager.CreateAsync(adminUser, adminPassword);
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }

                // Create Normal User
                var userEmail = "user@rungroop.kz";
                var userPassword = "User@1234";

                if (await userManager.FindByEmailAsync(userEmail) == null)
                {
                    var normalUser = new AppUser
                    {
                        UserName = "user",
                        Email = userEmail,
                        EmailConfirmed = true,
                        Address = new Address { Street = "User Street", City = "Astana", State = "KZ" }
                    };
                    await userManager.CreateAsync(normalUser, userPassword);
                    await userManager.AddToRoleAsync(normalUser, "User");
                }
            }
        }
    }
}
