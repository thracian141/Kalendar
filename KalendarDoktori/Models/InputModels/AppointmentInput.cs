namespace KalendarDoktori.Models.InputModels {
	public class AppointmentInput {
		public int DoctorId { get; set; }
		public int PatientId { get; set; }
		public DateOnly Date { get; set; }
		public TimeOnly Time { get; set; }
	}
}
