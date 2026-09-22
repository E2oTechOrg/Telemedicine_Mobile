namespace telemedicine;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(VideoCallPage), typeof(VideoCallPage));
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}