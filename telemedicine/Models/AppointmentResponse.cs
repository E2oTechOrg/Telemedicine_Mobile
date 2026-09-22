namespace telemedicine.Models;

public class AppointmentResponse
{
    public int TotalCount { get; set; }

    public List<AppointmentModel> Appointments { get; set; } = new();
}

public class AppointmentModel
{
    public int AppointmentId { get; set; }

    public string DoctorName { get; set; }

    public string PatientName { get; set; }

    public DateTime Date { get; set; }

    public string MeetingLink { get; set; }

    public string Status { get; set; }
}

public class UpdateAppointmentStatusRequest
{
    public int AppointmentId { get; set; }
    public string Status { get; set; }
}