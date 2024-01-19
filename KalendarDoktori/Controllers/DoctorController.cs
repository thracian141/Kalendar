using KalendarDoktori.Data;
using KalendarDoktori.Models;
using KalendarDoktori.Models.InputModels;
using KalendarDoktori.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KalendarDoktori.Controllers {
	[ApiController]
	[Route("doctor")]
	public class DoctorController : ControllerBase {
		private readonly UserManager<ApplicationUser> _userManager;
		private ApplicationDbContext _db;
		private readonly ILogger<DoctorController> _logger;

        public DoctorController(UserManager<ApplicationUser> userManager,
			ILogger<DoctorController> logger, ApplicationDbContext db) {
			_userManager=userManager;
			_logger=logger;
			_db=db;
        }

		[HttpGet("listPatients")]
		[Authorize(Roles = ApplicationRoles.Admin+","+ApplicationRoles.Doctor)]
		public async Task<IActionResult> ListPatients() {
			var patients = await _userManager.GetUsersInRoleAsync(ApplicationRoles.User);
			_logger.LogInformation("PATIENT COUNT: " + patients.Count.ToString());
			if (patients.Count == 0) {
				return NotFound("No patients found.");
			}
			return new JsonResult(new { patients });
		}

		[HttpPost("addAppointment")]
		[Authorize(Roles = ApplicationRoles.Admin + "," + ApplicationRoles.Doctor)]
		public async Task<IActionResult> AddAppointment([FromBody] AppointmentInput model) {
			_logger.LogInformation(model.Date.ToString());
			Appointment appointment = new Appointment {
				DoctorId=model.DoctorId,
				Doctor=await _db.Users.FindAsync(model.DoctorId),
				PatientId=model.PatientId,
				Patient=await _db.Users.FindAsync(model.PatientId),
				DateAndHour =model.Date.ToDateTime(model.Time)
			};

			await _db.Appointments.AddAsync(appointment);
			await _db.SaveChangesAsync();

			return Ok(appointment.Id.ToString());
		}
	}
}
