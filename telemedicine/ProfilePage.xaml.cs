using telemedicine.Models;
using telemedicine.Services;

namespace telemedicine;

public partial class ProfilePage : ContentPage
{
    private readonly ApiService _apiService;

    public ProfilePage()
    {
        InitializeComponent();

        _apiService = new ApiService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Footer.SetSelectedTab("Profile"); 

        await LoadDoctorProfile();
    }

    private async Task LoadDoctorProfile()
    {
        try
        {
            int doctorId = Preferences.Get("DoctorId", 0);

            if (doctorId == 0)
                return;

            var doctor = await _apiService
                .GetAsync<DoctorModel>(
                    $"api/Doctor/{doctorId}");

            if (doctor == null)
                return;

            DoctorNameLabel.Text =
                $"Dr. {doctor.Name}";

            DepartmentLabel.Text =
                doctor.Department;

            ExperienceLabel.Text =
                $"{doctor.Experience} Year(s) Experience";

            HospitalLabel.Text =
                doctor.Hospital;

            AddressLabel.Text =
                doctor.Address;

            PhoneLabel.Text =
                doctor.Phone;

            double rating =
     Convert.ToDouble(doctor.Ratings);

            RatingsLabel.Text =
                $"{rating:0.0}";

            SuccessRateLabel.Text =
                $"{doctor.SuccessRate}%";

            StatusLabel.Text =
                doctor.Status;

            if (!string.IsNullOrWhiteSpace(
                doctor.ProfileImage))
            {
                DoctorImage.Source =
                    ImageSource.FromUri(
                        new Uri(
                            "https://app-bgm-hospital-b4hbefbzd4ffbhhj.canadacentral-01.azurewebsites.net/"
                            + doctor.ProfileImage));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }
}