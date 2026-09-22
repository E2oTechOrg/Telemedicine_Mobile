namespace telemedicine.Models;

public class PrescriptionResponse
{
    public int TotalPrescriptions { get; set; }

    public List<PrescriptionModel> Prescriptions { get; set; } = new();
}

public class PrescriptionModel
{
    public int PrescriptionId { get; set; }

    public int CompanyId { get; set; }

    public int DoctorId { get; set; }

    public int PatientId { get; set; }

    public string PatientName { get; set; }

    public DateTime Date { get; set; }

    public string Notes { get; set; }

    public List<PrescriptionPointModel> PrescriptionPoints { get; set; }

    public string FirstPoint =>
        PrescriptionPoints != null &&
        PrescriptionPoints.Count > 0
            ? PrescriptionPoints[0].PointsMessage
            : "";
}

public class PrescriptionPointModel
{
    public int PrescriptionPointsId { get; set; }

    public int CompanyId { get; set; }

    public int PrescriptionId { get; set; }

    public string PointsMessage { get; set; }
}


public class DoctorPatientSummaryResponse
{
    public int TotalPatients { get; set; }
    public List<PatientModels> Patients { get; set; }
}

public class PatientModels
{
    public int PatientId { get; set; }
    public string PatientName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public string BloodGroup { get; set; }
    public string Disease { get; set; }
    public string Department { get; set; }
    public string Status { get; set; }
}

public class PrescriptionRequest
{
    public int PrescriptionId { get; set; }
    public int CompanyId { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; }
    public DateTime Date { get; set; }
    public string Notes { get; set; }

    public List<PrescriptionPointRequest> PrescriptionPoints { get; set; }
}

public class PrescriptionPointRequest
{
    public int PrescriptionPointsId { get; set; }
    public int CompanyId { get; set; }
    public int PrescriptionId { get; set; }
    public string PointsMessage { get; set; }
}
