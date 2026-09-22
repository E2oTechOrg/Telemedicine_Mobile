namespace telemedicine;

public partial class VideoCallPage : ContentPage
{
    public VideoCallPage(string meetingUrl)
    {
        InitializeComponent();

        MeetingWebView.Source =
            new UrlWebViewSource
            {
                Url = meetingUrl
            };
    }

    private async void CloseMeeting(
        object sender,
        TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}