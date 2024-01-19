using KalendarDoktori.Data;
using KalendarDoktori.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace KalendarDoktori.Utilities
{
    public class DbInitializer : IDbInitializer
    {
		private UserManager<ApplicationUser> _userManager;
		private RoleManager<IdentityRole<int>> _roleManager;
		private ApplicationDbContext _db;

		public DbInitializer(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole<int>> roleManager,ApplicationDbContext context) {
			_userManager=userManager;
			_roleManager=roleManager;
			_db=context;
		}

		public void Initialize()
        {
			if (!_roleManager.Roles.Any()) {
				_roleManager.CreateAsync(new IdentityRole<int> { Name=ApplicationRoles.Admin}).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole<int> { Name=ApplicationRoles.Doctor }).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole<int> { Name=ApplicationRoles.User }).GetAwaiter().GetResult();
			}
			if (_roleManager.Roles.Any() && !_userManager.Users.Any()) {
				var sysAdmin = new ApplicationUser {
					UserName="sysadmin",
					Email="admin@docalendar.com",
					EmailConfirmed=true
				};

				var result = _userManager.CreateAsync(sysAdmin,"Parola123?").GetAwaiter().GetResult();
			}
			if (_userManager.Users.FirstOrDefaultAsync().GetAwaiter().GetResult().UserName == "sysadmin") {
				var sysadmin = _db.Users.FirstOrDefault();
				_userManager.AddToRoleAsync(sysadmin,ApplicationRoles.Admin).GetAwaiter().GetResult();
			}
		}
    }
}
