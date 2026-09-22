using CommunityToolkit.Maui.Core.Primitives;
using telemedicine.Models;
using telemedicine.Services;

namespace telemedicine;

public partial class HomePage : ContentPage
{
    private readonly ApiService _apiService;

    public HomePage()
    {
        InitializeComponent();

        _apiService = new ApiService();

        HomeDatePicker.Date = DateTime.Today;

        UpdateDate(DateTime.Today);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Footer.SetSelectedTab("Home");

        await LoadDoctorProfile();

        await LoadAppointments();

        await LoadPatients();
    }

    void ShowLoader()
    {
        LoadingOverlay.IsVisible = true;
    }

    void HideLoader()
    {
        LoadingOverlay.IsVisible = false;
    }

    private async Task LoadDoctorProfile()
    {
        try
        {
            int doctorId =
                Preferences.Get("DoctorId", 0);

            if (doctorId == 0)
                return;

            var doctor =
                await _apiService.GetAsync<DoctorModel>(
                    $"api/Doctor/{doctorId}");

            if (doctor == null)
                return;

            DoctorNameLabel.Text =
                $"Dr. {doctor.Name}";

            DepartmentLabel.Text =
                doctor.Department;

            if (!string.IsNullOrWhiteSpace(
                doctor.ProfileImage))
            {
                DoctorImage.Source =
                    ImageSource.FromUri(
                        new Uri(
                            $"https://app-bgm-hospital-b4hbefbzd4ffbhhj.canadacentral-01.azurewebsites.net/{doctor.ProfileImage}"
                        ));
            }
        }
        catch
        {
        }
    }

    private async Task LoadAppointments()
    {
        try
        {

            ShowLoader();

            int companyId =
                Preferences.Get("CompanyId", 0);

            int doctorId =
                Preferences.Get("DoctorId", 0);

            string date =
                HomeDatePicker.Date
                .ToString("yyyy-MM-dd");

            var response =
                await _apiService.GetAsync<AppointmentResponse>(
                    $"api/Appointment/company/{companyId}/doctor/{doctorId}/date/{date}");

            if (response == null)
                return;

            MeetingCountLabel.Text =
                $"{response.TotalCount} Meetings";

            MeetingCollection.ItemsSource =
                response.Appointments;
        }
        catch
        {
        }
        finally
        {
            HideLoader();
        }
    }

    private async Task LoadPatients()
    {
        try
        {
            int companyId =
                Preferences.Get("CompanyId", 0);

            int doctorId =
                Preferences.Get("DoctorId", 0);

            var response =
                await _apiService.GetAsync<PatientSummaryResponse>(
                    $"api/Appointment/doctor-patient-summary?companyId={companyId}&doctorId={doctorId}");

            if (response == null)
                return;

            PatientCountLabel.Text =
                $"{response.TotalPatients} Patients";

            PatientCollection.ItemsSource =
                response.Patients;
        }
        catch
        {
        }
    }

    private async void JoinMeetingClicked(
        object sender,
        EventArgs e)
    {
        var button = sender as Button;

        var appointment =
            button?.BindingContext
            as AppointmentModel;

        if (appointment == null)
            return;

        await Navigation.PushAsync(
                new VideoCallPage(
                    appointment.MeetingLink));
    }

    private async void HomeDatePicker_DateSelected(
        object sender,
        DateChangedEventArgs e)
    {
        UpdateDate(e.NewDate);

        await LoadAppointments();
    }

    private void UpdateDate(DateTime date)
    {
        DayNumberLabel.Text =
            date.Day.ToString();

        DayNameLabel.Text =
            date.ToString("ddd")
                .ToUpper();
    }
}