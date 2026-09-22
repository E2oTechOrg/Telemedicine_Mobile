using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Storage;
using telemedicine.Models;
using telemedicine.Services;

namespace telemedicine;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _apiService;

    public LoginPage()
    {
        InitializeComponent();

        _apiService = new ApiService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            int doctorId = Preferences.Get("DoctorId", 0);

            if (doctorId > 0)
            {
                await Navigation.PushAsync(new HomePage());
            }
        }
        catch
        {
        }
    }

    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry &&
            entry.Parent is Frame frame)
        {
            frame.BorderColor = Colors.Black;
        }
    }

    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry &&
            entry.Parent is Frame frame)
        {
            frame.BorderColor = Color.FromArgb("#D1D5DB");
        }
    }

    void ShowLoader()
    {
        LoadingOverlay.IsVisible = true;
    }

    void HideLoader()
    {
        LoadingOverlay.IsVisible = false;
    }


    private async void LoginClicked(object sender, EventArgs e)
    {
        try
        {
            bool isValid = true;

            EmailFrame.BorderColor = Color.FromArgb("#D1D5DB");
            PasswordFrame.BorderColor = Color.FromArgb("#D1D5DB");

            if (string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                EmailFrame.BorderColor = Colors.Red;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                PasswordFrame.BorderColor = Colors.Red;
                isValid = false;
            }

            if (!isValid)
            {
                await Toast.Make(
                    "Please fill all required fields",
                    ToastDuration.Short)
                    .Show();

                return;
            }

            // SHOW LOADER
            ShowLoader();

            LoginButton.IsEnabled = false;

            var request = new LoginRequest
            {
                Email = EmailEntry.Text.Trim(),
                Password = PasswordEntry.Text.Trim()
            };

            var response =
                await _apiService.PostAsync<LoginResponse>(
                    "api/Doctor/login",
                    request);

            // HIDE LOADER
            HideLoader();

            LoginButton.IsEnabled = true;

            if (response != null &&
    response.Success &&
    response.Data != null)
            {
                Preferences.Remove("CompanyId");
                Preferences.Remove("DoctorId");
                Preferences.Remove("DoctorName");

                Preferences.Set(
                    "CompanyId",
                    response.Data.CompanyId);

                Preferences.Set(
                    "DoctorId",
                    response.Data.DoctorId);

                Preferences.Set(
                    "DoctorName",
                    response.Data.Name ?? "");

                string fcmToken =
                    Preferences.Get(
                        "FCM_TOKEN",
                        string.Empty);

                if (!string.IsNullOrWhiteSpace(fcmToken))
                {
                    await _apiService.SaveFcmTokenAsync(
                        response.Data.DoctorId,
                        fcmToken);
                }

                await Toast.Make(
                    $"Welcome Dr. {response.Data.Name}",
                    ToastDuration.Short)
                    .Show();

                await Application.Current.MainPage.Navigation.PushAsync(
                    new MainPage());
            }
            else
            {
                await Toast.Make(
                    "Invalid Email or Password",
                    ToastDuration.Short)
                    .Show();
            }
        }
        catch (Exception ex)
        {
            HideLoader();

            LoginButton.IsEnabled = true;

            await Toast.Make(
                ex.Message,
                ToastDuration.Long)
                .Show();
        }
    }
}