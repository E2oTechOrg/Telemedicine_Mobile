using Microsoft.Maui.Storage;

namespace telemedicine.Services;

public class NotificationService
{
    public string GetToken()
    {
        return Preferences.Get(
            "FCM_TOKEN",
            string.Empty);
    }
}