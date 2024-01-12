using KalendarDoktori.Data;
using KalendarDoktori.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace KalendarDoktori.Utilities
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async void Initialize()
        {
            if (_db.Roles.Any(x => x.Name == ApplicationRoles.Admin)) return;

            await _roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Admin));
            await _roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Doctor));
            await _roleManager.CreateAsync(new IdentityRole(ApplicationRoles.User));

            await _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "sysadmin",
                Email = "admin@gmail.com",
                EmailConfirmed = true

            }, "Parola123?");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == "admin@gmail.com");
            await _userManager.AddToRoleAsync(user, ApplicationRoles.Admin);
        }
    }
}
