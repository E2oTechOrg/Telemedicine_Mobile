namespace telemedicine;

public static class DeepLinkService
{
    public static void Handle(string url)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(url))
                return;

            if (!url.Contains("join/"))
                return;

            string roomCode = url.Split("join/")[1];

            string meetingUrl =
                $"https://p2p.mirotalk.com/join/{roomCode}";

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync(
                    nameof(VideoCallPage),
                    true,
                    new Dictionary<string, object>
                    {
                        ["meetingUrl"] = meetingUrl
                    });
            });
        }
        catch { }
    }
}