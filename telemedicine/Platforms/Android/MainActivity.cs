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
    const int MediaPermissionRequestCode = 1002;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

#if DEBUG
        Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
#endif

        RequestNotificationPermission();
        RequestMediaPermissions();

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
                    new[] { Manifest.Permission.PostNotifications },
                    NotificationPermissionRequestCode);
            }
        }
    }

    private void RequestMediaPermissions()
    {
        var permissionsNeeded = new List<string>();

        if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != Permission.Granted)
            permissionsNeeded.Add(Manifest.Permission.Camera);

        if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.RecordAudio) != Permission.Granted)
            permissionsNeeded.Add(Manifest.Permission.RecordAudio);

        if (permissionsNeeded.Count > 0)
        {
            ActivityCompat.RequestPermissions(this, permissionsNeeded.ToArray(), MediaPermissionRequestCode);
        }
    }
}