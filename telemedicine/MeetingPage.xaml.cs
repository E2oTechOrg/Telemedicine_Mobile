namespace telemedicine;

public partial class MeetingPage : ContentPage
{
    public MeetingPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Permissions.RequestAsync<Permissions.Camera>();
        await Permissions.RequestAsync<Permissions.Microphone>();
    }

    // GENERATE LINK
    private void GenerateMeetingLink(object sender, EventArgs e)
    {
        string roomCode = Guid.NewGuid().ToString("N")[..10];

        // ?? MUST BE CUSTOM SCHEME FOR DEEP LINKING
        string meetingLink =
            $"telemedicine://join/{roomCode}";

        MeetingLinkEntry.Text = meetingLink;
    }

    // COPY
    private async void CopyMeetingLink(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(MeetingLinkEntry.Text))
            return;

        await Clipboard.Default.SetTextAsync(MeetingLinkEntry.Text);
        await DisplayAlert("Copied", "Link copied!", "OK");
    }

    // SHARE WHATSAPP
    private async void ShareWhatsApp(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(MeetingLinkEntry.Text))
            return;

        string text =
            $"Join my telemedicine meeting:\n\n{MeetingLinkEntry.Text}";

        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = text,
            Title = "Meeting Link"
        });
    }

    // EMAIL
    private async void ShareEmail(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(MeetingLinkEntry.Text))
            return;

        var email = new EmailMessage
        {
            Subject = "Telemedicine Meeting",
            Body = $"Join here:\n{MeetingLinkEntry.Text}"
        };

        await Email.Default.ComposeAsync(email);
    }

    // JOIN MEETING (SAFE VERSION)
    private async void JoinMeeting(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(MeetingLinkEntry.Text))
        {
            await DisplayAlert("Alert", "Generate meeting first", "OK");
            return;
        }

        if (!MeetingLinkEntry.Text.Contains("join/"))
        {
            await DisplayAlert("Error", "Invalid meeting link", "OK");
            return;
        }

        string roomCode =
            MeetingLinkEntry.Text.Split("join/").Last();

        string meetingUrl =
            $"https://p2p.mirotalk.com/join/{roomCode}";

        await Shell.Current.GoToAsync(
            nameof(VideoCallPage),
            true,
            new Dictionary<string, object>
            {
                ["meetingUrl"] = meetingUrl
            });
    }
}