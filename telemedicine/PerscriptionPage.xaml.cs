using CommunityToolkit.Maui.Core.Primitives;
using telemedicine.Models;
using telemedicine.Services;

namespace telemedicine;

public partial class PerscriptionPage : ContentPage
{
    private readonly ApiService _apiService;

    private List<PrescriptionModel> _prescriptions =
        new();

    private List<PrescriptionModel> AllPrescriptions =
    new List<PrescriptionModel>();

    public PerscriptionPage()
    {
        InitializeComponent();

        _apiService = new ApiService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Footer.SetSelectedTab("Prescription");

        await LoadPrescriptions();
    }

    void ShowLoader()
    {
        LoadingOverlay.IsVisible = true;
    }

    void HideLoader()
    {
        LoadingOverlay.IsVisible = false;
    }
    private async Task LoadPrescriptions()
{
    try
    {
            ShowLoader();

        int companyId = Preferences.Get("CompanyId", 0);
        int doctorId = Preferences.Get("DoctorId", 0);

        var response =
            await _apiService.GetAsync<PrescriptionResponse>(
                $"api/Prescription/company/{companyId}/doctor/{doctorId}");

        if (response != null)
        {

            AllPrescriptions = response.Prescriptions;

            PrescriptionCollection.ItemsSource =
                AllPrescriptions;

            PrescriptionCountLabel.Text =
                response.TotalPrescriptions.ToString();
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

    private void SearchPrescription_TextChanged(
    object sender,
    TextChangedEventArgs e)
    {
        string searchText =
            e.NewTextValue?.ToLower()?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(searchText))
        {
            PrescriptionCollection.ItemsSource =
                AllPrescriptions;

            return;
        }

        var filteredList =
            AllPrescriptions.Where(x =>

                (!string.IsNullOrEmpty(x.PatientName) &&
                 x.PatientName.ToLower().Contains(searchText))

                ||

                x.PrescriptionId.ToString()
                .Contains(searchText)

                ||

                (!string.IsNullOrEmpty(x.FirstPoint) &&
                 x.FirstPoint.ToLower().Contains(searchText))

            ).ToList();

        PrescriptionCollection.ItemsSource =
            filteredList;
    }

    private async void EditPrescriptionClicked(object sender, EventArgs e)
{
    try
    {
            ShowLoader();
        Button button = sender as Button;

        if (button == null)
            return;

        PrescriptionModel prescription =
            button.CommandParameter as PrescriptionModel;

        if (prescription == null)
            return;

        await Navigation.PushAsync(
            new AddPrescriptionPage(prescription));
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

    private async void ViewPrescriptionClicked(
        object sender,
        EventArgs e)
    {
        Button button = (Button)sender;

        PrescriptionModel prescription =
            (PrescriptionModel)button.BindingContext;

        await DisplayAlert(
            "Prescription",
            prescription.Notes,
            "OK");
    }

    private async void SharePrescriptionClicked(object sender, EventArgs e)
    {
        try
        {
            Button button = sender as Button;

            if (button?.BindingContext is not PrescriptionModel prescription)
                return;

            ShowLoader();

            string url =
                $"https://app-bgm-hospital-b4hbefbzd4ffbhhj.canadacentral-01.azurewebsites.net/api/Prescription/download-pdf/{prescription.PrescriptionId}";

            using HttpClient client = new HttpClient();

            var pdfBytes = await client.GetByteArrayAsync(url);

            string filePath = Path.Combine(
                FileSystem.CacheDirectory,
                $"Prescription_{prescription.PrescriptionId}.pdf");

            File.WriteAllBytes(filePath, pdfBytes);

            await Share.RequestAsync(
                new ShareFileRequest
                {
                    Title = "Share Prescription",
                    File = new ShareFile(filePath)
                });
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

    private async void GotoAddPrescription(
        object sender,
        EventArgs e)
    {
        await Navigation.PushAsync(
            new AddPrescriptionPage());
    }
}