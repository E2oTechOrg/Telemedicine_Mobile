using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls;
using telemedicine.Models;
using telemedicine.Services;

namespace telemedicine;

public partial class AddPrescriptionPage : ContentPage
{
    private readonly ApiService _apiService;

    private int SelectedPatientId = 0;

    // Edit Mode
    private bool IsEditMode = false;
    private int PrescriptionId = 0;

    public AddPrescriptionPage(PrescriptionModel? prescription = null)
    {
        InitializeComponent();

        _apiService = new ApiService();

        LoadCurrentDate();

        if (prescription != null)
        {
            IsEditMode = true;

            PrescriptionId = prescription.PrescriptionId;

            SelectedPatientId = prescription.PatientId;

            FillPrescription(prescription);
        }
        else
        {
            IsEditMode = false;
            PrescriptionId = 0;
            SelectedPatientId = 0;
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

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

    private void FillPrescription(PrescriptionModel prescription)
    {
        PatientNameLabel.Text = prescription.PatientName;

        PatientPhoneLabel.Text = "";

        PatientInfoLabel.Text = "";

        SelectedPatientCard.IsVisible = true;

        NotesEditor.Text = prescription.Notes;

        PrescriptionContainer.Children.Clear();

        foreach (var point in prescription.PrescriptionPoints)
        {
            AddPrescriptionField(point.PointsMessage);
        }
    }

    private void AddPrescriptionField(string text = "")
    {
        Frame frame = new Frame
        {
            BackgroundColor = Color.FromArgb("#EEF5FF"),
            BorderColor = Color.FromArgb("#D6E4FF"),
            CornerRadius = 14,
            Padding = new Thickness(14, 6),
            HasShadow = false
        };

        Grid grid = new Grid
        {
            ColumnDefinitions =
        {
            new ColumnDefinition(GridLength.Star),
            new ColumnDefinition(GridLength.Auto)
        },
            ColumnSpacing = 10
        };

        Entry entry = new Entry
        {
            Text = text,
            Placeholder = "Enter medicine and dosage",
            BackgroundColor = Colors.Transparent
        };

        entry.Focused += PrescriptionEntry_Focused;
        entry.Unfocused += PrescriptionEntry_Unfocused;

        ImageButton delete = new ImageButton
        {
            Source = "delete.png",
            HeightRequest = 20,
            WidthRequest = 20,
            BackgroundColor = Colors.Transparent
        };

        delete.Clicked += (s, e) =>
        {
            if (PrescriptionContainer.Children.Count > 1)
                PrescriptionContainer.Children.Remove(frame);
        };

        grid.Add(entry);
        Grid.SetColumn(entry, 0);

        grid.Add(delete);
        Grid.SetColumn(delete, 1);

        frame.Content = grid;

        PrescriptionContainer.Children.Add(frame);
    }

    private void AddMorePrescriptionField(object sender, EventArgs e)
    {
        AddPrescriptionField();
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
                await _apiService.GetAsync<DoctorPatientSummaryResponse>(
                    $"api/Appointment/doctor-patient-summary?companyId={companyId}&doctorId={doctorId}");

            if (response != null)
            {
                PatientCollection.ItemsSource =
                    response.Patients;
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
    private void LoadCurrentDate()
    {
        DateTime today = DateTime.Today;

        DayLabel.Text = today.ToString("dddd");      // Monday
        DateLabel.Text = today.ToString("dd MMM yyyy"); // 08 Jul 2025
    }

    // OPEN POPUP
    private void OpenPatientPopup(object sender, EventArgs e)
    {
        PatientPopup.IsVisible = true;
    }

    // CLOSE POPUP
    private void ClosePatientPopup(object sender, EventArgs e)
    {
        PatientPopup.IsVisible = false;
    }

    // CHOOSE PATIENT
    private async void ChoosePatient(object sender, EventArgs e)
    {
        try
        {
            Button button = sender as Button;

            if (button == null)
                return;

            PatientModels patient =
                button.CommandParameter as PatientModels;

            if (patient == null)
            {
                await DisplayAlert(
                    "Error",
                    "Patient object is null",
                    "OK");
                return;
            }

            // Save Patient Id
            SelectedPatientId = patient.PatientId;

            // Fill Selected Patient Card
            PatientNameLabel.Text = patient.PatientName;
            PatientInfoLabel.Text = $"{patient.Gender} • {patient.Disease}";
            PatientPhoneLabel.Text = patient.Phone;

            SelectedPatientCard.IsVisible = true;

            // Close popup
            PatientPopup.IsVisible = false;

            await Toast.Make(
    $"{patient.PatientName} selected successfully",
    ToastDuration.Short
).Show();
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }
    // ADD NEW INPUT FIELD
  

    // DELETE DEFAULT FIELD
    private void DeletePrescriptionField(object sender, EventArgs e)
    {
        ImageButton button = sender as ImageButton;

        if (button?.Parent is Grid grid &&
            grid.Parent is Frame frame)
        {
            // DON'T DELETE LAST FIELD
            if (PrescriptionContainer.Children.Count > 1)
            {
                PrescriptionContainer.Children.Remove(frame);
            }
        }
    }

    // ENTRY FOCUS BORDER COLOR
    private void PrescriptionEntry_Focused(object sender, FocusEventArgs e)
    {
        Entry entry = sender as Entry;

        if (entry?.Parent is Grid grid &&
            grid.Parent is Frame frame)
        {
            frame.BorderColor = Colors.Black;
        }
    }

    // ENTRY UNFOCUS BORDER COLOR
    private void PrescriptionEntry_Unfocused(object sender, FocusEventArgs e)
    {
        Entry entry = sender as Entry;

        if (entry?.Parent is Grid grid &&
            grid.Parent is Frame frame)
        {
            frame.BorderColor = Color.FromArgb("#D6E4FF");
        }
    }

    // SAVE
    private async void SavePrescription(object sender, EventArgs e)
    {
        try
        {
            ShowLoader();

            if (SelectedPatientId == 0)
            {
                await DisplayAlert(
                    "Validation",
                    "Please select patient",
                    "OK");

                return;
            }

            int companyId = Preferences.Get("CompanyId", 0);

            int doctorId = Preferences.Get("DoctorId", 0);

            List<PrescriptionPointRequest> points = new();

            foreach (var child in PrescriptionContainer.Children)
            {
                if (child is Frame frame &&
                   frame.Content is Grid grid)
                {
                    Entry entry =
                        grid.Children
                            .OfType<Entry>()
                            .FirstOrDefault();

                    if (entry != null &&
                       !string.IsNullOrWhiteSpace(entry.Text))
                    {
                        points.Add(
                            new PrescriptionPointRequest
                            {
                                PrescriptionPointsId = 0,
                                CompanyId = companyId,
                                PrescriptionId = PrescriptionId,
                                PointsMessage = entry.Text.Trim()
                            });
                    }
                }
            }

            var request = new PrescriptionRequest
            {
                PrescriptionId = PrescriptionId,
                CompanyId = companyId,
                DoctorId = doctorId,
                PatientId = SelectedPatientId,
                PatientName = PatientNameLabel.Text,
                Date = DateTime.Now,
                Notes = NotesEditor.Text,
                PrescriptionPoints = points
            };

            if (IsEditMode)
            {
                await _apiService.PutAsync(
    "api/Prescription",
    request);

                await Toast.Make(
                    "Prescription updated successfully",
                    ToastDuration.Short)
                    .Show();
            }
            else
            {
                await _apiService.PostAsync<object>(
                    "api/Prescription",
                    request);

                await Toast.Make(
                    "Prescription saved successfully",
                    ToastDuration.Short)
                    .Show();
            }

            await Navigation.PopAsync();
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
}