namespace telemedicine.Components;

public partial class FooterView : ContentView
{
    public FooterView()
    {
        InitializeComponent();

        // Default Selected Tab
    }


    public void SetSelectedTab(string tabName)
    {
        SetActiveTab(tabName);
    }

    // RESET ALL TABS
    private void ResetTabs()
    {
        // LABEL COLORS
        HomeLabel.TextColor = Color.FromArgb("#6B7280");
        PatientsLabel.TextColor = Color.FromArgb("#6B7280");
        PrescriptionLabel.TextColor = Color.FromArgb("#6B7280");
        MeetingLabel.TextColor = Color.FromArgb("#6B7280");
        ProfileLabel.TextColor = Color.FromArgb("#6B7280");

        // DEFAULT ICONS
        HomeIcon.Source = "home.png";
        PatientsIcon.Source = "patient.png";
        PrescriptionIcon.Source = "prescription.png";
        MeetingIcon.Source = "logout.png";
        ProfileIcon.Source = "profile.png";
    }

    // ACTIVE TAB STYLE
    private void SetActiveTab(string tabName)
    {
        ResetTabs();

        switch (tabName)
        {
            case "Home":
                HomeLabel.TextColor = Color.FromArgb("#7a2257");
                HomeIcon.Source = "homeblue.png";
                break;

            case "Patients":
                PatientsLabel.TextColor = Color.FromArgb("#7a2257");
                PatientsIcon.Source = "patientblue.png";
                break;

            case "Prescription":
                PrescriptionLabel.TextColor = Color.FromArgb("7a2257");
                PrescriptionIcon.Source = "prescriptionblue.png";
                break;

            case "Meeting":
                MeetingLabel.TextColor = Color.FromArgb("#7a2257");
                MeetingIcon.Source = "logoutblue.png";
                break;

            case "Profile":
                ProfileLabel.TextColor = Color.FromArgb("#7a2257");
                ProfileIcon.Source = "profileblue.png";
                break;
        }
    }

    // HOME
    private async void GotoHome(object sender, TappedEventArgs e)
    {
        SetActiveTab("Home");

        await Application.Current.MainPage.Navigation.PushAsync(new HomePage());
    }

    // PATIENTS
    private async void GotoPatients(object sender, TappedEventArgs e)
    {
        SetActiveTab("Patients");

        await Application.Current.MainPage.Navigation.PushAsync(new AppointmentPage());
    }

    // E-PRESCRIPTION
    private async void GotoPrescription(object sender, TappedEventArgs e)
    {
        SetActiveTab("Prescription");

        await Application.Current.MainPage.Navigation.PushAsync(new PerscriptionPage());
    }

    // MEETING
    private async void Logout(object sender, TappedEventArgs e)
    {
        bool answer = await Application.Current.MainPage.DisplayAlert(
            "Logout",
            "Are you sure you want to logout?",
            "Yes",
            "No");

        if (!answer)
            return;

        try
        {
            Preferences.Clear();
            SecureStorage.RemoveAll();

            await Application.Current.MainPage.Navigation.PushAsync(
    new MainPage());
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    // PROFILE
    private async void GotoProfile(object sender, TappedEventArgs e)
    {
        SetActiveTab("Profile");

        await Application.Current.MainPage.Navigation.PushAsync(new ProfilePage());
    }
}