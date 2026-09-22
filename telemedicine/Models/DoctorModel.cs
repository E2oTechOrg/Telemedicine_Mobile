namespace telemedicine.Models;

public class DoctorModel
{
    public int DoctorId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public DateTime JoiningDate { get; set; }

    public int Experience { get; set; }

    public string Address { get; set; } = string.Empty;

    public double Ratings { get; set; }

    public double SuccessRate { get; set; }

    public string Hospital { get; set; } = string.Empty;

    public string ProfileImage { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public int CompanyId { get; set; }
}