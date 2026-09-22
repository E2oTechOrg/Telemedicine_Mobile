namespace telemedicine.Models;

public class DeviceTokenRequest
{
    public int DoctorId { get; set; }

    public string FcmToken { get; set; } = string.Empty;
}