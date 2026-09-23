using Microsoft.Maui.Devices;

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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        DeviceDisplay.Current.KeepScreenOn = true;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        DeviceDisplay.Current.KeepScreenOn = false;
    }

    private async void CloseMeeting(
        object sender,
        TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}