namespace telemedicine.Models;

public class LoginResponse
{
    public bool Success { get; set; }

    public DoctorData? Data { get; set; }
}

public class DoctorData
{
    public int DoctorId { get; set; }

    public string? Name { get; set; }

    public string? Gender { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? Phone { get; set; }

    public string? Department { get; set; }

    public DateTime JoiningDate { get; set; }

    public int Experience { get; set; }

    public string? Address { get; set; }

    // CHANGE THESE
    public double Ratings { get; set; }

    public double SuccessRate { get; set; }

    public string? Hospital { get; set; }

    public string? ProfileImage { get; set; }

    public string? Status { get; set; }

    public int CompanyId { get; set; }
}