using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Primitives;
using telemedicine.Models;
using telemedicine.Services;

namespace telemedicine;

public partial class AppointmentPage : ContentPage
{
    private readonly ApiService _apiService;
    private AppointmentModelDto _selectedAppointment;
    private List<AppointmentModelDto> _appointments = new();

    public AppointmentPage()
    {
        InitializeComponent();

        _apiService = new ApiService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Footer.SetSelectedTab("Patients");

        await LoadAppointments();
    }

    void ShowLoader()
    {
        LoadingOverlay.IsVisible = true;
    }

    void HideLoader()
    {
        LoadingOverlay.IsVisible = false;
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

            if (companyId == 0 || doctorId == 0)
                return;

            var response =
                await _apiService.GetAsync<AppointmentResponseDto>(
                    $"api/Appointment/company/{companyId}/doctor/{doctorId}");

            if (response != null)
            {
                AppointmentCountLabel.Text =
                    response.TotalAppointments.ToString();

                _appointments =
                    response.Appointments ??
                    new List<AppointmentModelDto>();

                AppointmentCollection.ItemsSource =
                    _appointments;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
        finally
        {
            HideLoader();
        }
    }

    private async void StatusClicked(object sender, EventArgs e)
    {
        try
        {
            Button button = sender as Button;

            if (button == null)
                return;

            _selectedAppointment =
                button.CommandParameter as AppointmentModelDto;

            if (_selectedAppointment == null)
                return;

            string selectedStatus =
                await DisplayActionSheet(
                    "Select Appointment Status",
                    "Cancel",
                    null,
                    "Pending",
                    "Confirmed",
                    "Completed",
                    "Cancelled",
                    "Rescheduled");

            if (selectedStatus == null ||
                selectedStatus == "Cancel")
                return;

            bool confirm =
                await DisplayAlert(
                    "Confirm",
                    $"Change status to '{selectedStatus}' ?",
                    "OK",
                    "Cancel");

            if (!confirm)
                return;

            await UpdateAppointmentStatus(
                _selectedAppointment.AppointmentId,
                selectedStatus);
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    private async Task UpdateAppointmentStatus(
    int appointmentId,
    string status)
    {
        try
        {
            ShowLoader();

            var request = new
            {
                appointmentId = appointmentId,
                status = status
            };

            await _apiService.PutAsync(
                "api/Appointment/update-status",
                request);

            await Toast.Make(
     "Appointment status updated successfully",
     ToastDuration.Short,
     14)
 .Show();

            await LoadAppointments();
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
        finally
        {
            HideLoader();
        }
    }

    private async void SearchAppointment_TextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        try
        {
            string searchText =
                e.NewTextValue?.Trim().ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                AppointmentCollection.ItemsSource =
                    _appointments;

                return;
            }

            var filteredList =
                _appointments
                .Where(x =>

                    (!string.IsNullOrEmpty(x.PatientName) &&
                     x.PatientName.ToLower().Contains(searchText))

                    ||

                    (!string.IsNullOrEmpty(x.Phone) &&
                     x.Phone.ToLower().Contains(searchText))

                    ||

                    (!string.IsNullOrEmpty(x.Email) &&
                     x.Email.ToLower().Contains(searchText))

                    ||

                    (!string.IsNullOrEmpty(x.Status) &&
                     x.Status.ToLower().Contains(searchText))

                    ||

                    (!string.IsNullOrEmpty(x.Gender) &&
                     x.Gender.ToLower().Contains(searchText))

                    ||

                    x.AppointmentId
                     .ToString()
                     .Contains(searchText)
                )
                .ToList();

            AppointmentCollection.ItemsSource =
                filteredList;
        }
        catch
        {
        }
    }

    private async void JoinMeetingClicked(
       object sender,
       EventArgs e)
    {
        try
        {
            Button button = sender as Button;

            AppointmentModelDto appointment =
                button?.BindingContext as AppointmentModelDto;

            if (appointment == null)
                return;

            if (string.IsNullOrWhiteSpace(
                appointment.MeetingLink))
            {
                await DisplayAlert(
                    "Info",
                    "Meeting link not available.",
                    "OK");
                return;
            }

            await Navigation.PushAsync(
                new VideoCallPage(
                    appointment.MeetingLink));

            System.Diagnostics.Debug.WriteLine($"MEETING URL >>> {appointment.MeetingLink}");

            await Navigation.PushAsync(
                new VideoCallPage(
                    appointment.MeetingLink));

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