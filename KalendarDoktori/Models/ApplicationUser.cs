using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KalendarDoktori.Models
{
    public class ApplicationUser : IdentityUser<int> {
        [Key]
        public override int Id { get; set; }
        [Required]
        public override string UserName { get; set; }
    }
}
