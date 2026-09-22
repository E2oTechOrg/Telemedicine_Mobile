using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using Firebase.Messaging;
using Microsoft.Maui.Storage;

namespace telemedicine.Platforms.Android;

[Service(Exported = true)]
[IntentFilter(
    new[]
    {
        "com.google.firebase.MESSAGING_EVENT"
    })]
public class MyFirebaseMessagingService
    : FirebaseMessagingService
{
    public override void OnNewToken(
        string token)
    {
        base.OnNewToken(token);

        Preferences.Set(
            "FCM_TOKEN",
            token);

        System.Diagnostics.Debug.WriteLine(
            $"FCM TOKEN: {token}");
    }

    public override void OnMessageReceived(
        RemoteMessage message)
    {
        base.OnMessageReceived(message);

        string title =
            message.GetNotification()?.Title
            ?? "Telemedicine";

        string body =
            message.GetNotification()?.Body
            ?? "";

        ShowNotification(
            title,
            body);
    }

    private void ShowNotification(
        string title,
        string body)
    {
        string channelId =
            "telemedicine_channel";

        var manager =
            (NotificationManager)
            GetSystemService(
                NotificationService);

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channel =
                new NotificationChannel(
                    channelId,
                    "Telemedicine",
                    NotificationImportance.High);

            manager.CreateNotificationChannel(
                channel);
        }

        var builder =
            new NotificationCompat.Builder(
                this,
                channelId)
            .SetContentTitle(title)
            .SetContentText(body)
            .SetAutoCancel(true)
            .SetSmallIcon(
                Resource.Mipmap.appicon);

        manager.Notify(
            1,
            builder.Build());
    }
}