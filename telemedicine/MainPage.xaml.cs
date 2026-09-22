using Microsoft.Maui.Storage;

namespace telemedicine;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        bool isStarted =
            Preferences.Get("IsStarted", false);

        if (isStarted)
        {
            await Navigation.PushAsync(
                new LoginPage());
        }
    }

    private async void GetStarted_Clicked(
        object sender,
        EventArgs e)
    {
        // Save value in local storage
        Preferences.Set(
            "IsStarted",
            true);

        await Navigation.PushAsync(
            new LoginPage());
    }
}