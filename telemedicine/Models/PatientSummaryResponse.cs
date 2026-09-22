namespace telemedicine.Models;

public class PatientSummaryResponse
{
    public int TotalPatients { get; set; }

    public List<PatientModel> Patients { get; set; } = new();
}

public class PatientModel
{
    public int PatientId { get; set; }

    public string PatientName { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string Gender { get; set; }

    public string BloodGroup { get; set; }

    public string Disease { get; set; }

    public string Status { get; set; }

    public string Department { get; set; }
}