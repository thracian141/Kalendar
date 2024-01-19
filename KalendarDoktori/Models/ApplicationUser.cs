using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace KalendarDoktori.Models
{
    public class ApplicationUser : IdentityUser<int> {
        [Key]
		[Required]
		public override int Id { get; set; }
        [Required]
        public override string UserName { get; set; }
	}
}
