using KalendarDoktori.Models;
using KalendarDoktori.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KalendarDoktori.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int> { Id = 1, Name = ApplicationRoles.Admin, NormalizedName = ApplicationRoles.Admin.ToUpper() },
                new IdentityRole<int> { Id = 2, Name = ApplicationRoles.Doctor, NormalizedName = ApplicationRoles.Doctor.ToUpper() },
                new IdentityRole<int> { Id = 3, Name = ApplicationRoles.User, NormalizedName = ApplicationRoles.User.ToUpper() }
            );
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
