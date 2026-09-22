using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;

namespace telemedicine;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop,
    Exported = true,
    ConfigurationChanges =
        ConfigChanges.ScreenSize |
        ConfigChanges.Orientation |
        ConfigChanges.UiMode |
        ConfigChanges.ScreenLayout |
        ConfigChanges.SmallestScreenSize |
        ConfigChanges.Density
)]
[IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[]
    {
        Intent.CategoryDefault,
        Intent.CategoryBrowsable
    },
    DataScheme = "telemedicine",
    DataHost = "join"
)]
public class MainActivity : MauiAppCompatActivity
{
    const int NotificationPermissionRequestCode = 1001;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        RequestNotificationPermission();

        HandleIntent(Intent);
    }

    protected override void OnNewIntent(Intent? intent)
    {
        base.OnNewIntent(intent);

        if (intent != null)
        {
            HandleIntent(intent);
        }
    }

    private void HandleIntent(Intent intent)
    {
        if (intent?.Data != null)
        {
            DeepLinkService.Handle(intent.Data.ToString());
        }
    }

    private void RequestNotificationPermission()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
        {
            if (ContextCompat.CheckSelfPermission(
                    this,
                    Manifest.Permission.PostNotifications)
                != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(
                    this,
                    new[]
                    {
                        Manifest.Permission.PostNotifications
                    },
                    NotificationPermissionRequestCode);
            }
        }
    }
}