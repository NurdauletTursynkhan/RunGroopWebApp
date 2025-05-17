using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Race> Races { get; set; } = null!;
        public DbSet<Club> Clubs { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<State> States { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
    }
}
