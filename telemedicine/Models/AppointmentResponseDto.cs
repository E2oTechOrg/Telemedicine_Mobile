namespace telemedicine.Models;

public class AppointmentResponseDto
{
    public int TotalAppointments { get; set; }

    public List<AppointmentModelDto> Appointments { get; set; } = new();
}

public class AppointmentModelDto
{
    public int AppointmentId { get; set; }

    public int CompanyId { get; set; }

    public int DoctorId { get; set; }

    public string DoctorName { get; set; }

    public int PatientId { get; set; }

    public string PatientName { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string Gender { get; set; }

    public string BloodGroup { get; set; }

    public DateTime Dob { get; set; }

    public string PatientStatus { get; set; }

    public DateTime Date { get; set; }

    public string MeetingLink { get; set; }

    public string Status { get; set; }

    public string Note { get; set; }

    public string AppointmentType { get; set; }

    public int TokenNumber { get; set; }

    public decimal ConsultationFee { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public int Age =>
        DateTime.Today.Year - Dob.Year -
        (Dob.Date > DateTime.Today.AddYears(-(DateTime.Today.Year - Dob.Year)) ? 1 : 0);
}