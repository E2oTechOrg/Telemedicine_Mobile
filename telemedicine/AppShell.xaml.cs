namespace telemedicine
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(VideoCallPage), typeof(VideoCallPage));
        }
    }
}
