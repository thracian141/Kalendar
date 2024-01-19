using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KalendarDoktori.Models {
	public class Appointment {
		[Key]
		public int Id { get; set; }
		[Required]
		public int PatientId { get; set; }
		[ForeignKey("PatientId")]
		public virtual ApplicationUser Patient { get; set; }
		[Required]
		public int DoctorId { get; set; }
		[ForeignKey("DoctorId")]
		public virtual ApplicationUser Doctor { get; set; }
		[Required]
		public DateTime DateAndHour { get; set; }
	}
}
