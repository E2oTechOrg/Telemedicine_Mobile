using Microsoft.Maui.Storage;

namespace telemedicine;

public partial class AddPatientPage : ContentPage
{
    public AddPatientPage()
    {
        InitializeComponent();
    }

    private async void UploadPhoto_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select Patient Image",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                PatientProfileImage.Source = ImageSource.FromFile(result.FullPath);
            }
        }
        catch (Exception)
        {
            await DisplayAlert("Error", "Unable to select image.", "OK");
        }
    }

    // ENTRY
    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry && entry.Parent is Frame frame)
        {
            frame.BorderColor = Colors.Black;
        }
    }

    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry && entry.Parent is Frame frame)
        {
            frame.BorderColor = Color.FromArgb("#D1D5DB");
        }
    }

    // PICKER
    private void Picker_Focused(object sender, FocusEventArgs e)
    {
        if (sender is Picker picker && picker.Parent is Frame frame)
        {
            frame.BorderColor = Colors.Black;
        }
    }

    private void Picker_Unfocused(object sender, FocusEventArgs e)
    {
        if (sender is Picker picker && picker.Parent is Frame frame)
        {
            frame.BorderColor = Color.FromArgb("#D1D5DB");
        }
    }

    // EDITOR
    private void Editor_Focused(object sender, FocusEventArgs e)
    {
        if (sender is Editor editor && editor.Parent is Frame frame)
        {
            frame.BorderColor = Colors.Black;
        }
    }

    private void Editor_Unfocused(object sender, FocusEventArgs e)
    {
        if (sender is Editor editor && editor.Parent is Frame frame)
        {
            frame.BorderColor = Color.FromArgb("#D1D5DB");
        }
    }
}